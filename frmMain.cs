using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPSW_to_iBoot
{
    public partial class frmMain : Form, IIpswLog
    {
        private static readonly Color ColGreen  = Color.FromArgb(0, 255, 65);
        private static readonly Color ColYellow = Color.FromArgb(255, 220, 0);
        private static readonly Color ColCyan   = Color.FromArgb(0, 225, 235);
        private static readonly Color ColOrange = Color.FromArgb(255, 165, 0);
        private static readonly Color ColRed    = Color.FromArgb(255, 80, 80);

        private readonly List<string> _files = new List<string>();
        private readonly List<IpswPair> _pairs = new List<IpswPair>();
        private bool _busy;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            btnBrowse.Click += delegate { Browse(); };
            btnRead.Click += delegate { Start(); };
            btnCopy.Click += delegate { Copy(); };
            btnClearList.Click += delegate { ClearList(); };
            btnClearLog.Click += delegate { if (!_busy) ClearLog(); };
            lvPairs.MouseDoubleClick += delegate { CopyTag(); };

            DragEnter += frmMain_DragEnter;
            DragDrop += frmMain_DragDrop;

            Buttons();

            LogColor("IPSW TO IBOOT", ColCyan);
            LogInfo("Ready. Pick one or more .ipsw files, or drop them on this window.");
            LogInfo("Everything happens on this computer. Nothing is sent anywhere.");
            LogInfo("The build is read out of the boot image itself, which is what a device reports.");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            MinimumSize = Size;
        }

        private void frmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (_busy) { e.Effect = DragDropEffects.None; return; }
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            if (_busy) return;
            var dropped = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (dropped != null) Take(dropped);
        }

        private void Browse()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Pick firmware";
                dlg.Filter = "Apple firmware (*.ipsw)|*.ipsw|All files (*.*)|*.*";
                dlg.Multiselect = true;
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                Take(dlg.FileNames);
            }
        }

        private void Take(string[] paths)
        {
            var added = new List<string>();
            foreach (string p in paths)
            {
                if (string.IsNullOrWhiteSpace(p)) continue;
                if (Directory.Exists(p))
                {
                    foreach (string f in Directory.GetFiles(p, "*.ipsw", SearchOption.TopDirectoryOnly)) added.Add(f);
                    continue;
                }
                if (File.Exists(p)) added.Add(p);
            }

            _files.Clear();
            foreach (string f in added) if (!_files.Contains(f)) _files.Add(f);

            txtFiles.Text = _files.Count == 1
                ? _files[0]
                : (_files.Count == 0 ? "" : _files.Count + " files picked");

            LogEnsureLineStart();
            if (_files.Count == 0) LogWarn("Nothing usable in that selection.");
            else
            {
                LogKV("Picked", _files.Count + (_files.Count == 1 ? " file" : " files"));
                foreach (string f in _files) LogInfo("   " + Path.GetFileName(f));
            }
            Buttons();
        }

        private void Start()
        {
            if (_busy || _files.Count == 0) return;
            var work = new List<string>(_files);
            _busy = true;
            Buttons();

            Task.Run(() =>
            {
                var found = new List<IpswPair>();
                int ok = 0;
                foreach (string f in work)
                {
                    LogEnsureLineStart();
                    LogColor(new string('-', 46), ColGreen);
                    IpswRead r;
                    try { r = Ipsw.Read(f, this); }
                    catch (Exception ex)
                    {
                        LogErr(Path.GetFileName(f) + ": " + ex.Message);
                        continue;
                    }
                    if (r.Pairs.Count > 0) ok++;
                    found.AddRange(r.Pairs);
                }

                BeginInvoke(new Action(() =>
                {
                    foreach (IpswPair p in found) Add(p);
                    LogEnsureLineStart();
                    if (found.Count == 0) LogErr("No boot loader build could be read.");
                    else LogOk("Read " + ok + " of " + work.Count + ", " + found.Count + " boot loader "
                             + (found.Count == 1 ? "build" : "builds") + " listed.");
                    _busy = false;
                    Buttons();
                }));
            });
        }

        private void Add(IpswPair p)
        {
            foreach (ListViewItem old in lvPairs.Items)
            {
                var had = old.Tag as IpswPair;
                if (had != null && string.Equals(had.Tag, p.Tag, StringComparison.OrdinalIgnoreCase)
                                && string.Equals(had.Ios, p.Ios, StringComparison.Ordinal)) return;
            }

            var it = new ListViewItem(p.File);
            it.SubItems.Add(p.Device);
            it.SubItems.Add(p.Ios);
            it.SubItems.Add(p.Build);
            it.SubItems.Add(p.Tag);
            it.SubItems.Add(p.Source);
            it.Tag = p;
            if (p.Source == "manifest") it.ForeColor = BrandPalette.Warn;
            lvPairs.Items.Add(it);
            _pairs.Add(p);
        }

        private void ClearList()
        {
            if (_busy) return;
            lvPairs.Items.Clear();
            _pairs.Clear();
            Buttons();
        }

        private void Copy()
        {
            var rows = new List<IpswPair>();
            foreach (ListViewItem it in lvPairs.SelectedItems)
            {
                var p = it.Tag as IpswPair;
                if (p != null) rows.Add(p);
            }
            if (rows.Count == 0) rows = _pairs;
            if (rows.Count == 0) return;

            var sb = new StringBuilder();
            foreach (IpswPair p in rows)
                sb.AppendLine(p.Tag + "\t" + p.Ios + "\t" + p.Build + "\t" + p.Device + "\t" + p.Source);
            try { Clipboard.SetText(sb.ToString()); LogOk("Copied " + rows.Count + "."); }
            catch (Exception ex) { LogErr("Could not copy: " + ex.Message); }
        }

        private void CopyTag()
        {
            if (lvPairs.SelectedItems.Count != 1) { Copy(); return; }
            var p = lvPairs.SelectedItems[0].Tag as IpswPair;
            if (p == null) return;
            try { Clipboard.SetText(p.Tag); LogOk("Copied " + p.Tag + "."); }
            catch (Exception ex) { LogErr("Could not copy: " + ex.Message); }
        }

        private void Buttons()
        {
            btnBrowse.Enabled = !_busy;
            btnRead.Enabled = !_busy && _files.Count > 0;
            btnCopy.Enabled = !_busy && _pairs.Count > 0;
            btnClearList.Enabled = !_busy && _pairs.Count > 0;
            btnClearLog.Enabled = !_busy;
            Cursor = _busy ? Cursors.AppStarting : Cursors.Default;
        }

        private void LogEnsureLineStart()
        {
            if (rtLogView.InvokeRequired) { rtLogView.Invoke(new Action(LogEnsureLineStart)); return; }
            int n = rtLogView.TextLength;
            if (n == 0) return;
            if (!rtLogView.Text.EndsWith("\n"))
            {
                rtLogView.SelectionStart = n; rtLogView.SelectionLength = 0;
                rtLogView.SelectionColor = ColGreen;
                rtLogView.AppendText("\r\n");
            }
        }

        private void LogColor(string message, Color color)
        {
            if (rtLogView.InvokeRequired)
            { rtLogView.Invoke(new Action<string, Color>(LogColor), message, color); return; }
            rtLogView.SelectionStart = rtLogView.TextLength;
            rtLogView.SelectionLength = 0;
            rtLogView.SelectionColor = color;
            rtLogView.AppendText(message + "\r\n");
            rtLogView.SelectionColor = ColGreen;
            rtLogView.ScrollToCaret();
        }

        private void LogInfo(string message) { LogColor(message, ColGreen); }
        private void LogOk(string message)   { LogColor(message, ColYellow); }
        private void LogWarn(string message) { LogColor(message, ColOrange); }
        private void LogErr(string message)  { LogColor(message, ColRed); }

        private void LogKV(string key, string value)
        {
            if (rtLogView.InvokeRequired)
            { rtLogView.Invoke(new Action<string, string>(LogKV), key, value); return; }
            rtLogView.SelectionStart = rtLogView.TextLength;
            rtLogView.SelectionLength = 0;
            rtLogView.SelectionColor = ColGreen;
            string k = key ?? "";
            if (k.Length > 0 && !k.EndsWith(" ")) k += k.EndsWith(":") ? "  " : ":  ";
            rtLogView.AppendText(k);
            rtLogView.SelectionColor = ColCyan;
            rtLogView.AppendText((value ?? "") + "\r\n");
            rtLogView.SelectionColor = ColGreen;
            rtLogView.ScrollToCaret();
        }

        private void LogStep(string msg)
        {
            if (rtLogView.InvokeRequired) { rtLogView.Invoke(new Action<string>(LogStep), msg); return; }
            rtLogView.SelectionStart = rtLogView.TextLength; rtLogView.SelectionLength = 0;
            rtLogView.SelectionColor = ColGreen;
            rtLogView.AppendText("> " + msg + "...  ");
            rtLogView.ScrollToCaret();
        }

        private void LogStepDone(string word, bool ok)
        {
            if (rtLogView.InvokeRequired) { rtLogView.Invoke(new Action<string, bool>(LogStepDone), word, ok); return; }
            rtLogView.SelectionStart = rtLogView.TextLength; rtLogView.SelectionLength = 0;
            rtLogView.SelectionColor = ok ? ColYellow : ColRed;
            rtLogView.AppendText(word + "\r\n");
            rtLogView.SelectionColor = ColGreen;
            rtLogView.ScrollToCaret();
        }

        private void ClearLog()
        {
            if (rtLogView.InvokeRequired) { rtLogView.Invoke(new Action(ClearLog)); return; }
            rtLogView.Clear();
        }

        void IIpswLog.Step(string msg)             { LogStep(msg); }
        void IIpswLog.Done(string word, bool ok)   { LogStepDone(word, ok); }
        void IIpswLog.Info(string msg)             { LogInfo(msg); }
        void IIpswLog.Warn(string msg)             { LogWarn(msg); }
        void IIpswLog.Err(string msg)              { LogErr(msg); }
        void IIpswLog.Kv(string key, string value) { LogKV(key, value); }
    }
}
