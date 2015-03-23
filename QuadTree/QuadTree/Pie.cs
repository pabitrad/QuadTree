using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace QuadTree
{
    public class Pie : Shape
    {
        public double CentreX { get; set; }
        public double CentreY { get; set; }
        public double Radius { get; set; }
        public double Rotation { get; set; }
        public double Angle { get; set; }

        public Pie()
        {
            //Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("black"));//"#4386D8"
            Stroke = Brushes.Black;
            StrokeThickness = 1;
        }

        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            Point startPoint = new Point(CentreX, CentreY);
            //this.
            Point outerArcStartPoint = ComputeCartesianCoordinate(Rotation, Radius);
            outerArcStartPoint.Offset(CentreX, CentreY);

            Point outerArcEndPoint = ComputeCartesianCoordinate(180/*Angle*/, Radius);
            outerArcEndPoint.Offset(CentreX, CentreY);

            //bool largeArc = Angle > 180.0;
            Size outerArcSize = new Size(Radius, Radius);

            Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("black"));//"#4386D8"
            context.BeginFigure(startPoint, true, true);
            context.LineTo(outerArcStartPoint, true, true);
            context.ArcTo(outerArcEndPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);

            Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("white"));//"#4386D8"
            context.ArcTo(outerArcStartPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);

            //outerArcStartPoint = outerArcEndPoint;
        }
    }
}
