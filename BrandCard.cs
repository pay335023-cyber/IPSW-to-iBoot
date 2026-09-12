using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IPSW_to_iBoot
{

    public class BrandCard : Panel
    {
        private Color _face = Color.FromArgb(249, 250, 252);
        private Color _edge = Color.FromArgb(226, 230, 238);

        public int Radius { get; set; }

        public Color FaceColor
        {
            get { return _face; }
            set { _face = value; BackColor = value; Invalidate(); }
        }
        public Color BorderColor
        {
            get { return _edge; }
            set { _edge = value; Invalidate(); }
        }

        public BrandCard()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Radius = 1;
            BackColor = _face;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.Clear(Parent != null ? Parent.BackColor : Color.White);

                var box = new Rectangle(0, 0, Width - 1, Height - 1);
                int rad = Math.Max(0, Math.Min(Radius, Math.Min(Width, Height) / 2 - 1));
                using (GraphicsPath path = RoundedRect(box, rad))
                {
                    using (var face = new SolidBrush(_face)) g.FillPath(face, path);
                    using (var edge = new Pen(_edge)) g.DrawPath(edge, path);
                }
            }
            catch { }
            base.OnPaint(e);
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var p = new GraphicsPath();
            if (radius <= 0) { p.AddRectangle(r); return p; }
            int d = radius * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
