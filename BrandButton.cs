using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IPSW_to_iBoot
{

    public enum BrandButtonKind { Auto, Brand, Warn, Danger, Gold }

    public class BrandButton : Button
    {
        private static readonly Color Brand  = Color.FromArgb(69, 89, 237);
        private static readonly Color BrandH = Color.FromArgb(92, 110, 245);
        private static readonly Color BrandD = Color.FromArgb(52, 70, 210);
        private static readonly Color Warn   = Color.FromArgb(235, 150, 30);
        private static readonly Color WarnH  = Color.FromArgb(245, 165, 48);
        private static readonly Color WarnD  = Color.FromArgb(205, 128, 20);
        private static readonly Color Danger = Color.FromArgb(208, 55, 55);
        private static readonly Color DangerH = Color.FromArgb(224, 72, 72);
        private static readonly Color DangerD = Color.FromArgb(180, 45, 45);

        private static readonly Color Gold   = Color.FromArgb(198, 160, 45);
        private static readonly Color GoldH  = Color.FromArgb(216, 178, 66);
        private static readonly Color GoldD  = Color.FromArgb(168, 134, 34);

        private static readonly HashSet<string> DangerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "btnDeleteFile", "btnUninstall" };
        private static readonly HashSet<string> WarnNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "btnScanVirus" };

        private const int Radius = 4;
        private bool _hover, _pressed;

        public BrandButtonKind Kind { get; set; }
        public string Glyph { get; set; }

        public bool GrayWhenDisabled { get; set; }

        public BrandButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9F);
            Cursor = Cursors.Hand;
            Kind = BrandButtonKind.Auto;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }

        private BrandButtonKind Effective()
        {
            if (Kind != BrandButtonKind.Auto) return Kind;
            if (!string.IsNullOrEmpty(Name) && DangerNames.Contains(Name)) return BrandButtonKind.Danger;
            if (!string.IsNullOrEmpty(Name) && WarnNames.Contains(Name)) return BrandButtonKind.Warn;
            return BrandButtonKind.Brand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                Color back, hover, down;
                switch (Effective())
                {
                    case BrandButtonKind.Danger: back = Danger; hover = DangerH; down = DangerD; break;
                    case BrandButtonKind.Warn:   back = Warn;   hover = WarnH;   down = WarnD;   break;
                    case BrandButtonKind.Gold:   back = Gold;   hover = GoldH;   down = GoldD;   break;
                    default:                     back = Brand;  hover = BrandH;  down = BrandD;  break;
                }
                bool grayed = !Enabled && GrayWhenDisabled;
                Color fill = !Enabled ? (grayed ? Color.FromArgb(226, 229, 235) : Color.FromArgb(150, back))
                                      : (_pressed ? down : (_hover ? hover : back));
                Color ink = grayed ? Color.FromArgb(138, 144, 156) : ForeColor;

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear((Parent != null) ? Parent.BackColor : BackColor);

                if (Width > Radius * 2 && Height > Radius * 2)
                {
                    using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), Radius))
                    using (SolidBrush brush = new SolidBrush(fill))
                        g.FillPath(brush, path);
                }
                string text = Text ?? "";
                if (!string.IsNullOrEmpty(Glyph))
                {
                    using (Font gf = new Font("Segoe MDL2 Assets", Font.Size + 1f, FontStyle.Regular, GraphicsUnit.Point))
                    {
                        const TextFormatFlags m = TextFormatFlags.NoPadding;
                        Size gsz = TextRenderer.MeasureText(g, Glyph, gf, new Size(int.MaxValue, int.MaxValue), m);
                        bool hasText = text.Length > 0;
                        Size tsz = hasText ? TextRenderer.MeasureText(g, text, Font, new Size(int.MaxValue, int.MaxValue), m) : Size.Empty;
                        int gap = hasText ? 7 : 0;
                        int total = gsz.Width + gap + tsz.Width;
                        int startX = Math.Max(4, (Width - total) / 2);
                        TextRenderer.DrawText(g, Glyph, gf, new Point(startX, (Height - gsz.Height) / 2), ink, m);
                        if (hasText)
                            TextRenderer.DrawText(g, text, Font, new Point(startX + gsz.Width + gap, (Height - tsz.Height) / 2), ink, m);
                    }
                }
                else
                {
                    TextRenderer.DrawText(g, text, Font, ClientRectangle, ink,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
            }
            catch { }
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
