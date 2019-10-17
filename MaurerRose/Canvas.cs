using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MaurerRose
{
    public partial class Canvas : UserControl
    {
        /// <summary>
        /// Points to hold data for draw lines
        /// </summary>
        List<Point> _points = new List<Point>();

        /// <summary>
        /// Pen used for the Monroe
        /// </summary>
        private Pen _penMonroe = new Pen(Color.MediumVioletRed, 1);
        
        /// <summary>
        /// Pen used for the sin
        /// </summary>
        private Pen _penSin = new Pen(Color.Orange, 3);

        /// <summary>
        /// Brush used for canvas
        /// </summary>
        private SolidBrush _drawBrush = new SolidBrush(Color.Orange);

        /// <summary>
        /// Timer for animation
        /// </summary>
        private Timer _animationTimer = new Timer();

        /// <summary>
        /// Random for animation
        /// </summary>
        private Random _rnd = new Random();

        private double n = 3;
        private double d = 47;

        public Canvas()
        {
            InitializeComponent();

            DoubleBuffered = true;

            _animationTimer.Interval = 1000;
            _animationTimer.Tick += _animationTimer_Tick;
            //_animationTimer.Start();
        }

        /// <summary>
        /// Factor to fill canvas
        /// </summary>
        private int _sizeFactor = 1;

        private void _animationTimer_Tick(object sender, EventArgs e)
        {
            n += _rnd.NextDouble();
            d += _rnd.NextDouble() ;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Fancy graphics
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Fresh start
            _points.Clear();

            // Calculation for translation
            int middleX = this.Width / 2;
            int middleY = this.Height / 2;
            
            // Calculate for Maurer
            for (float i = 0; i <= 361; i += 1f)
            {
                double k = i * d;
                double r = Math.Sin(DegreeToRadian(n * k)) * _sizeFactor;

                int x = (int)(r * Math.Sin(DegreeToRadian(k))) + middleX;
                int y = (int)(r * Math.Cos(DegreeToRadian(k))) + middleY;

                _points.Add(new Point(x, y));
            }

            // Draw the Maurer rose
            e.Graphics.DrawLines(_penMonroe, _points.ToArray());

            // Fresh start
            _points.Clear();

            for (float i = 0; i <= 361; i += 1f)
            {
                float k = i;
                double r = Math.Sin(DegreeToRadian(n * k)) * _sizeFactor;

                int x = (int)(r * Math.Sin(DegreeToRadian(k))) + middleX;
                int y = (int)(r * Math.Cos(DegreeToRadian(k))) + middleY;

                _points.Add(new Point(x, y));
            }

            // Draw the main path
            e.Graphics.DrawLines(_penSin, _points.ToArray());
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _sizeFactor = (int)((this.Height + this.Width) / 2) / 3;
            this.Invalidate();
        }

        /// <summary>
        /// Get the radian angle from degrees
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Angle in radian</returns>
        public double DegreeToRadian(float angle)
        {
            return Math.PI * angle / 180f;
        }

        /// <summary>
        /// Get the radian angle from degrees
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Angle in radian</returns>
        public double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180d;
        }
    }
}
