using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace IPSW_to_iBoot
{
    internal sealed class IpswPair
    {
        public string Tag = "";
        public string Ios = "";
        public string Build = "";
        public string Device = "";
        public string Source = "";
        public string Entry = "";
        public string File = "";
    }

    internal sealed class IpswRead
    {
        public string Path = "";
        public string FileName = "";
        public string Version = "";
        public string Build = "";
        public string Device = "";
        public string NameVersion = "";
        public string NameBuild = "";
        public string NameDevice = "";
        public bool ImageTagFound;
        public string Error = "";
        public List<IpswPair> Pairs = new List<IpswPair>();
    }

    internal interface IIpswLog
    {
        void Step(string msg);
        void Done(string word, bool ok);
        void Info(string msg);
        void Warn(string msg);
        void Err(string msg);
        void Kv(string key, string value);
    }

    internal static class Ipsw
    {
        private static readonly Regex RxTag =
            new Regex(@"\b[A-Za-z]*Boot-[0-9][0-9.~]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex RxImagePath =
            new Regex(@"Firmware/[^<]*iBoot\.[^<]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex RxImageEntry =
            new Regex(@"(^|/)iBoot\.[^/]+\.(im4p|img3|img2)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex RxNameVerBuild =
            new Regex(@"_(\d+(?:\.\d+){1,3})_([0-9]{1,2}[A-Z][0-9A-Za-z]+)_", RegexOptions.Compiled);

        private static readonly Regex RxNameDevice =
            new Regex(@"^([A-Za-z]+\d+,\d+)_", RegexOptions.Compiled);

        public static IpswRead Read(string path, IIpswLog log)
        {
            var r = new IpswRead();
            r.Path = path ?? "";
            r.FileName = System.IO.Path.GetFileName(r.Path);

            ReadName(r, log);

            log.Step("Opening " + r.FileName);
            ZipArchive zip;
            try
            {
                zip = ZipFile.OpenRead(r.Path);
            }
            catch (Exception ex)
            {
                log.Done("FAIL", false);
                r.Error = ex.Message;
                log.Err(ex.Message);
                return r;
            }
            log.Done("OK", true);

            using (zip)
            {
                string manifestName;
                string manifest = ReadManifest(zip, out manifestName);
                if (manifest == null)
                {
                    r.Error = "no BuildManifest.plist and no Restore.plist inside this file";
                    log.Err(r.Error);
                    return r;
                }

                log.Step("Reading " + manifestName);
                r.Version = PlistValue(manifest, "ProductVersion");
                r.Build = PlistValue(manifest, "ProductBuildVersion");
                r.Device = FirstProductType(manifest);
                log.Done(r.Version.Length > 0 ? "OK" : "FAIL", r.Version.Length > 0);

                if (r.Version.Length == 0 && r.NameVersion.Length > 0)
                {
                    r.Version = r.NameVersion;
                    log.Warn("The firmware does not state a version. Using the one in the file name.");
                }
                if (r.Build.Length == 0) r.Build = r.NameBuild;
                if (r.Device.Length == 0) r.Device = r.NameDevice;

                if (r.Version.Length > 0) log.Kv("iOS version", r.Version);
                if (r.Build.Length > 0) log.Kv("Build", r.Build);
                if (r.Device.Length > 0) log.Kv("Device", r.Device);

                if (r.NameVersion.Length > 0 && r.Version.Length > 0 && r.NameVersion != r.Version)
                    log.Warn("The file name says " + r.NameVersion + " but the firmware says " + r.Version + ". The firmware wins.");

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var paths = ImagePaths(zip, manifest, log);

                foreach (string p in paths)
                {
                    log.Step("Reading boot image " + Short(p));
                    byte[] bin = Entry(zip, p);
                    if (bin == null) { log.Done("FAIL", false); continue; }

                    var tags = Tags(bin);
                    log.Done(tags.Count > 0 ? "OK" : "NONE", tags.Count > 0);
                    if (tags.Count == 0)
                    {
                        log.Warn("That image carries no readable tag. It is encrypted, or from an era that stores none.");
                        continue;
                    }
                    foreach (string t in tags)
                    {
                        log.Kv("Boot loader", t);
                        r.ImageTagFound = true;
                        if (!seen.Add(t)) continue;
                        r.Pairs.Add(NewPair(t, r, "image", p));
                    }
                }

                if (!r.ImageTagFound)
                {
                    log.Step("Reading the boot loader named in the firmware list");
                    var mtags = Tags(Encoding.UTF8.GetBytes(manifest));
                    log.Done(mtags.Count > 0 ? "OK" : "FAIL", mtags.Count > 0);
                    if (mtags.Count == 0)
                    {
                        if (r.Error.Length == 0) r.Error = "no boot loader build could be read from this file";
                        log.Err(r.Error);
                    }
                    else
                    {
                        log.Warn("Read from the firmware list, not from the image. Since iOS 26 the two differ, and a device reports the image one, so check this before publishing it.");
                        foreach (string t in mtags)
                        {
                            log.Kv("Boot loader", t);
                            if (!seen.Add(t)) continue;
                            r.Pairs.Add(NewPair(t, r, "manifest", manifestName));
                        }
                    }
                }
            }

            return r;
        }

        private static IpswPair NewPair(string tag, IpswRead r, string source, string entry)
        {
            return new IpswPair
            {
                Tag = tag,
                Ios = r.Version,
                Build = r.Build,
                Device = r.Device,
                Source = source,
                Entry = entry,
                File = r.FileName,
            };
        }

        private static void ReadName(IpswRead r, IIpswLog log)
        {
            var m = RxNameVerBuild.Match(r.FileName);
            if (m.Success) { r.NameVersion = m.Groups[1].Value; r.NameBuild = m.Groups[2].Value; }
            var d = RxNameDevice.Match(r.FileName);
            if (d.Success) r.NameDevice = d.Groups[1].Value;

            if (r.NameVersion.Length == 0 && r.NameDevice.Length == 0) return;

            log.Step("Reading the file name");
            log.Done("OK", true);
            if (r.NameDevice.Length > 0) log.Kv("Name says device", r.NameDevice);
            if (r.NameVersion.Length > 0) log.Kv("Name says iOS", r.NameVersion);
            if (r.NameBuild.Length > 0) log.Kv("Name says build", r.NameBuild);
        }

        private static string ReadManifest(ZipArchive zip, out string name)
        {
            string[] wanted = new string[] { "BuildManifest.plist", "Restore.plist" };
            foreach (string want in wanted)
            {
                foreach (ZipArchiveEntry e in zip.Entries)
                {
                    if (!string.Equals(e.FullName, want, StringComparison.OrdinalIgnoreCase)) continue;
                    byte[] b = Bytes(e);
                    if (b == null) continue;
                    name = want;
                    return Encoding.UTF8.GetString(b);
                }
            }
            foreach (string want in wanted)
            {
                foreach (ZipArchiveEntry e in zip.Entries)
                {
                    if (!string.Equals(e.Name, want, StringComparison.OrdinalIgnoreCase)) continue;
                    byte[] b = Bytes(e);
                    if (b == null) continue;
                    name = e.FullName;
                    return Encoding.UTF8.GetString(b);
                }
            }
            name = "";
            return null;
        }

        private static List<string> ImagePaths(ZipArchive zip, string manifest, IIpswLog log)
        {
            var named = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Match m in RxImagePath.Matches(manifest))
            {
                string p = m.Value.Trim();
                if (p.Length > 0 && seen.Add(p)) named.Add(p);
            }

            if (named.Count == 0)
            {
                foreach (ZipArchiveEntry e in zip.Entries)
                    if (RxImageEntry.IsMatch(e.FullName) && seen.Add(e.FullName)) named.Add(e.FullName);
                if (named.Count > 0)
                    log.Info("The firmware list names no image paths, so the boot image was found by listing the file.");
            }

            var live = new List<string>();
            foreach (string p in named) if (Find(zip, p) != null) live.Add(p);

            if (live.Count == 0 && named.Count > 0)
                log.Warn("The firmware list names a boot image this file does not contain.");

            return live;
        }

        private static ZipArchiveEntry Find(ZipArchive zip, string path)
        {
            foreach (ZipArchiveEntry e in zip.Entries)
                if (string.Equals(e.FullName, path, StringComparison.OrdinalIgnoreCase)) return e;
            foreach (ZipArchiveEntry e in zip.Entries)
                if (e.FullName.EndsWith(path, StringComparison.OrdinalIgnoreCase)) return e;
            return null;
        }

        private static byte[] Entry(ZipArchive zip, string path)
        {
            ZipArchiveEntry e = Find(zip, path);
            return e == null ? null : Bytes(e);
        }

        private static byte[] Bytes(ZipArchiveEntry e)
        {
            try
            {
                using (var s = e.Open())
                using (var ms = new MemoryStream())
                {
                    s.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch { return null; }
        }

        public static List<string> Tags(byte[] bytes)
        {
            var found = new List<string>();
            if (bytes == null || bytes.Length == 0) return found;
            string s = Encoding.ASCII.GetString(bytes);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match m in RxTag.Matches(s))
            {
                string t = m.Value.Split('~')[0].TrimEnd('.');
                if (t.Length < 7) continue;
                if (seen.Add(t)) found.Add(t);
            }
            found.Sort(StringComparer.OrdinalIgnoreCase);
            return found;
        }

        private static string PlistValue(string plist, string key)
        {
            var m = Regex.Match(plist, "<key>" + Regex.Escape(key) + "</key>\\s*<string>([^<]*)</string>");
            return m.Success ? m.Groups[1].Value.Trim() : "";
        }

        private static string FirstProductType(string plist)
        {
            var m = Regex.Match(plist, "<key>SupportedProductTypes</key>\\s*<array>(.*?)</array>", RegexOptions.Singleline);
            if (m.Success)
            {
                var s = Regex.Match(m.Groups[1].Value, "<string>([^<]+)</string>");
                if (s.Success) return s.Groups[1].Value.Trim();
            }
            return PlistValue(plist, "ProductType");
        }

        private static string Short(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            int i = path.LastIndexOf('/');
            return i >= 0 && i < path.Length - 1 ? path.Substring(i + 1) : path;
        }
    }
}
