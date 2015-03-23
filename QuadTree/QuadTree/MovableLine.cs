using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Petzold.Media2D;
using System.Windows.Media;
using Microsoft.Expression.Interactivity.Layout;
using System.Windows.Controls;

namespace QuadTree
{
    public class MovableLine : ArrowLine
    {
        //public ArrowLine lineTobeAdded = new ArrowLine() { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = 3 };
        public Ellipse FrontConnector = new Ellipse() { Width = 8, Height = 8 };
        public Ellipse TailConnector = new Ellipse() { Width = 8, Height = 8 };
        private Canvas _canvas;

        public MovableLine(Canvas canvas)
        {
            _canvas = canvas;
            SolidColorBrush brush = new SolidColorBrush();

            // Describes the brush's color using RGB values.  
            // Each value has a range of 0-255.

            brush.Color = Color.FromArgb(255, 255, 255, 0);
            //brush.Opacity = 0.25;
            FrontConnector.Fill = brush;
            TailConnector.Fill = brush;

            FrontConnector.Visibility = System.Windows.Visibility.Hidden;
            TailConnector.Visibility = System.Windows.Visibility.Hidden;

            FrontConnector.RenderTransform = new TranslateTransform();
            TailConnector.RenderTransform = new TranslateTransform();
        }

        public  bool VisibleConnectors
        {
            set
            {
                if (value == true)
                {
                    FrontConnector.Visibility = System.Windows.Visibility.Visible;
                    TailConnector.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    FrontConnector.Visibility = System.Windows.Visibility.Hidden;
                    TailConnector.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        public void addToDragList()
        {
            MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
            dragBehavior.Attach(this);
            dragBehavior.Dragging += dragBehavior_Dragging;

            MouseDragElementBehavior dragBehaviorConnectorFront = new MouseDragElementBehavior();
            dragBehaviorConnectorFront.Attach(FrontConnector);
            dragBehaviorConnectorFront.Dragging += dragBehaviorConnectorFront_Dragging;

            MouseDragElementBehavior dragBehaviorConnectorTail = new MouseDragElementBehavior();
            dragBehaviorConnectorTail.Attach(TailConnector);
            dragBehaviorConnectorTail.Dragging += dragBehaviorConnectorTail_Dragging;
        }

        void dragBehavior_Dragging(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Vector diff = e.GetPosition(DisplayArea) - mouseStartPosition;

            MovableLine line = e.Source as MovableLine;
            TranslateTransform frotTrans = line.FrontConnector.RenderTransform as TranslateTransform;
            TranslateTransform tailTrans = line.TailConnector.RenderTransform as TranslateTransform;

            //if (frotTrans != null)
            //{
            //    frotTrans.X = diff.X;
            //    frotTrans.Y = diff.Y;
            //}

            //if (tailTrans != null)
            //{
            //    tailTrans.X = diff.X;
            //    tailTrans.Y = diff.Y;
            //}
        }
        void dragBehaviorConnectorTail_Dragging(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point pt = e.GetPosition(_canvas);
            this.X2 = pt.X;
            this.Y2 = pt.Y;
        }

        void dragBehaviorConnectorFront_Dragging(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point pt = e.GetPosition(_canvas);
            this.X1 = pt.X;
            this.Y1 = pt.Y;
        }
    }
}
