using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.Expression.Interactivity.Layout;

using Petzold.Media2D;

namespace QuadTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int NODE_DIMENSION = 15;
        private enum QuadTreeNode
        {
            None,
            White,
            Black,
            Gray,
            Arrow
        }

        private Point mouseStartPosition;

        MovableLine _animationLine = null;
        private QuadTreeNode _selectedNode = QuadTreeNode.None;

        private QuadTreeManager _manager = new QuadTreeManager();
        private ImageMatrix _imageMatrix;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Make sure you have saved your data before closing.");
            Close();
        }

        private void MenuItem_Click_LoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            //dlg.DefaultExt = ".png";
            dlg.Filter = "Text Files (.txt)|*.txt|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                _manager.loadData(dlg.FileName);
                _imageMatrix = new ImageMatrix(_manager.Points, _manager.Rows, _manager.Columns);

                displayImage();
            }
        }
		
		private void MenuItem_Click_LoadQuadTreeFromPreOrderText(object sender, RoutedEventArgs e)
		{
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            //dlg.DefaultExt = ".png";
            dlg.Filter = "Text Files (.txt)|*.txt";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
			if (result == true)
            {
                _manager.drwaQuadTree(DisplayArea, dlg.FileName);
			}
		}
		
        private void MenuItem_Click_DisplayImage(object sender, RoutedEventArgs e)
        {
            if (_manager.IsDataLoaded() == true)
            {
                displayImage();
            }
            else
            {
                MessageBox.Show("Please load image file.", "QuadTree");
            }
        }




        private void MenuItem_Click_DisplayQuadTree(object sender, RoutedEventArgs e)
        {
            if (_manager.IsDataLoaded() == true)
            {
                _manager.drwaQuadTree(DisplayArea);
            }
            else
            {
                MessageBox.Show("Please load image file.", "QuadTree");
            }
        }




        private void MenuItem_Click_FormatNodeLarge(object sender, RoutedEventArgs e)
        {
                    NODE_DIMENSION = 30;
        }


        private void MenuItem_Click_FormatNodeMedium(object sender, RoutedEventArgs e)
        {
            NODE_DIMENSION = 23;
        }


        private void MenuItem_Click_FormatNodeSmall(object sender, RoutedEventArgs e)
        {
            NODE_DIMENSION = 15;
        }

        private void MenuItem_Click_DrawWhiteNode(object sender, RoutedEventArgs e)
        {
            _selectedNode = QuadTreeNode.White;

        }

        private void MenuItem_Click_DrawBlackNode(object sender, RoutedEventArgs e)
        {
            _selectedNode = QuadTreeNode.Black;
        }

        private void MenuItem_Click_DrawGrayNode(object sender, RoutedEventArgs e)
        {
            _selectedNode = QuadTreeNode.Gray;
        }

        private void MenuItem_Click_DrawArrow(object sender, RoutedEventArgs e)
        {
            _selectedNode = QuadTreeNode.Arrow;
        }

        private void MenuItem_Click_SaveImage(object sender, RoutedEventArgs e)
        {
            if (DisplayImage.Children.Count > 0)
            {
                SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                //dlg.DefaultExt = ".png";
                dlg.Filter = "PNG Files (*.png)|*.png";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    Utility.SaveWindow(DisplayImage, 96, dlg.FileName);
                    MessageBox.Show("Image Saved sucessfully!");
                }
            }
            else
            {
                MessageBox.Show("There is no image to save.", "QuadTree");
            }
        }

        private void MenuItem_Click_SavePreOrderText(object sender, RoutedEventArgs e)
        {
            if (_manager.QuadTree != null)
            {
                string preOrdertext = _manager.generatePreOrderText();
                SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "Text Files (*.txt)|*.txt";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Utility.SaveText(preOrdertext, dlg.FileName);
                    MessageBox.Show("Pre-Order Text Saved sucessfully!");
                }
            }
            else
            {
                MessageBox.Show("There is no QuadTree to save.", "QuadTree");
            }
        }

        private void MenuItem_Click_SavePreOrderTextFromImage(object sender, RoutedEventArgs e)
        {
            if (_imageMatrix != null)
            {
                string preOrdertext = _imageMatrix.generatePreOrderText();
                SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "Text Files (*.txt)|*.txt";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Utility.SaveText(preOrdertext, dlg.FileName);
                    MessageBox.Show("Pre-Order Text Saved sucessfully!");
                }
            }
            else
            {
                MessageBox.Show("There is no Image Matrix to save.", "QuadTree");
            }

        }

        private void MenuItem_Click_SaveQuadTree(object sender, RoutedEventArgs e)
        {
            if (DisplayArea.Children.Count > 0)
            {
                SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                //dlg.DefaultExt = ".png";
                dlg.Filter = "PNG Files (*.png)|*.png";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    Utility.CreateBitmapFromVisual(DisplayArea, dlg.FileName);
                    MessageBox.Show("Image Saved sucessfully!");
                }
            }
            else
            {
                MessageBox.Show("There is no image to save.", "QuadTree");
            }
        }

        private void MenuItem_Click_CloseImage(object sender, RoutedEventArgs e)
        {
            DisplayImage.Children.Clear();
            DisplayImage.RowDefinitions.Clear();
            DisplayImage.ColumnDefinitions.Clear();

           _manager.cleraData(); // TO CLEAR EVERYTHING FOREVER
        }


        private void MenuItem_Click_CloseQuadTree(object sender, RoutedEventArgs e)
        {
            DisplayArea.Children.Clear();
            _manager.clearQuadTree();
        }

        private void MenuItem_Click_ImageToQuadTree(object sender, RoutedEventArgs e)
        {
            if (_manager.IsDataLoaded() == false)
            {
                MessageBox.Show("Please load image file.", "QuadTree");
                return;
            }
            _manager.drwaQuadTree(DisplayArea);
        }

        private void MenuItem_Click_QuadTreeToImage(object sender, RoutedEventArgs e)
        {
            _manager.convertQuadTreeToMatrix();
            displayImage();

            if (_manager.Points != null)
            {
                SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "Text Files (*.txt)|*.txt";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Utility.SaveMatrix(_manager.Points, dlg.FileName);
                    //MessageBox.Show("Pre-Order Text Saved sucessfully!");
                }
            }
        }

        private void displayArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Retrieve the coordinate of the mouse position.
            mouseStartPosition = e.GetPosition(DisplayArea);
            Point pt= e.GetPosition((UIElement)sender);
            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(DisplayArea, pt);
            if (!(result.VisualHit is Canvas))
            {
                return;
            }

            if (_selectedNode == QuadTreeNode.None)
            {
                return;
            }

            if (_selectedNode == QuadTreeNode.Arrow)
            {
                Point P1 = e.GetPosition(DisplayArea);
                _animationLine = new MovableLine(DisplayArea) { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = 1.5 };
                _animationLine.X1 = P1.X;
                _animationLine.Y1 = P1.Y;
                DisplayArea.Children.Add(_animationLine);

                return;
            }

            Shape renderShape = new Ellipse() { Width = NODE_DIMENSION, Height = NODE_DIMENSION };
            double pointX = e.GetPosition(DisplayArea).X - NODE_DIMENSION / 2;
            double pointY = e.GetPosition(DisplayArea).Y - NODE_DIMENSION / 2;

            LinearGradientBrush brush = new LinearGradientBrush();
            if (_selectedNode == QuadTreeNode.Black || _selectedNode == QuadTreeNode.White)
            {
                Color color = Colors.White;
                if (_selectedNode == QuadTreeNode.Black)
                {
                    color = Colors.Black;
                }

                brush.GradientStops.Add(new GradientStop(color, 1.0));
            }
            else if (_selectedNode == QuadTreeNode.Gray)
            {
                brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));

                Ellipse ellipse = renderShape as Ellipse;
                ellipse.RenderTransform = new RotateTransform(-45);
            }
            renderShape.Fill = brush;


            Canvas.SetLeft(renderShape, pointX);
            Canvas.SetTop(renderShape, pointY);
            DisplayArea.Children.Add(renderShape);

            addToDragList(renderShape, new Point(pointX, pointY));
            
            //string strPosition = "X = " + pointX + " ,Y = " + pointY;
            //MessageBox.Show(strPosition);
        }

        private void displayArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_animationLine == null)
            {
                return;
            }

            MovableLine lineTobeAdded = new MovableLine(DisplayArea) { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = 1.5 };
            lineTobeAdded.X1 = _animationLine.X1;
            lineTobeAdded.Y1 = _animationLine.Y1;

            lineTobeAdded.X2 = _animationLine.X2;
            lineTobeAdded.Y2 = _animationLine.Y2;

            DisplayArea.Children.Add(lineTobeAdded);
            DisplayArea.Children.Remove(_animationLine);
            _animationLine = null;

            lineTobeAdded.VisibleConnectors = true;

            Canvas.SetLeft(lineTobeAdded.FrontConnector, lineTobeAdded.X1);
            Canvas.SetTop(lineTobeAdded.FrontConnector, lineTobeAdded.Y1);

            Canvas.SetLeft(lineTobeAdded.TailConnector, lineTobeAdded.X2);
            Canvas.SetTop(lineTobeAdded.TailConnector, lineTobeAdded.Y2);

            DisplayArea.Children.Add(lineTobeAdded.FrontConnector);
            DisplayArea.Children.Add(lineTobeAdded.TailConnector);

            lineTobeAdded.addToDragList(); //Dragging
        }


        private void displayArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedNode != QuadTreeNode.Arrow)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed && _animationLine != null)
            {
                var P2 = e.GetPosition(DisplayArea);
                _animationLine.X2 = P2.X;
                _animationLine.Y2 = P2.Y;
            }
        }

        private void addToDragList(Shape renderShape, Point pt)
        {
            MouseDragElementBehavior dragBehaviorEllipse = new MouseDragElementBehavior();
            dragBehaviorEllipse.Attach(renderShape);

            TextBlock lable = new TextBlock { Text = "X = " + pt.X + ", Y = " + pt.Y };
            lable.RenderTransform = new TranslateTransform();

            Canvas.SetLeft(lable, pt.X + NODE_DIMENSION + 3);
            Canvas.SetTop(lable, pt.Y + (NODE_DIMENSION / 2 - 5));

            //DisplayArea.Children.Add(lable);
            renderShape.Tag = lable; // attach to Ellipse

            dragBehaviorEllipse.Dragging += dragBehaviorEllipse_Dragging;

            dragBehaviorEllipse.DragFinished += dragBehaviorEllipse_DragFinished; ;
        }

        void dragBehaviorEllipse_DragFinished(object sender, MouseEventArgs e)
        {
            Ellipse node = e.Source as Ellipse;
            Point curPoint = e.GetPosition(DisplayArea);
            TextBlock label = node.Tag as TextBlock;
            TranslateTransform labelTrans = label.RenderTransform as TranslateTransform;
            labelTrans.X = 0;
            labelTrans.Y = 0;

            Canvas.SetLeft(label, curPoint.X + NODE_DIMENSION + 3);
            Canvas.SetTop(label, curPoint.Y);
        }

        void dragBehaviorEllipse_Dragging(object sender, MouseEventArgs e)
        {
            Point curPoint = e.GetPosition(DisplayArea);
            Vector diff = curPoint - mouseStartPosition;
            Ellipse node = e.Source as Ellipse;
            if (node != null)
            {
                TextBlock label = node.Tag as TextBlock;
                TranslateTransform labelTrans = label.RenderTransform as TranslateTransform;
                labelTrans.X = diff.X;
                labelTrans.Y = diff.Y;

                label.Text = getLabelText(curPoint);
            }
        }

        private void addLineToDragList(MovableLine line)
        {

        }

        private string getLabelText(Point pt)
        {
            //Point labelPoint = new Point(pt.X, pt.Y + (NODE_DIMENSION / 2 - 5));
            string lableText = "X = " + pt.X + ", Y = " + pt.Y;

            return lableText;
        }

        private void displayImage()
        {
            if (DisplayImage.Children.Count > 0)
            {
                return;
            }

            int rows = _manager.Rows;
            int columns = _manager.Columns;

            Rectangle exampleRectangle = new Rectangle();
            exampleRectangle.Width = DisplayImage.Width;
            exampleRectangle.Height = DisplayImage.Height;

            int cellWidth = (int) DisplayImage.Width / rows;
            int cellHeight = (int) DisplayImage.Height / columns;
            
            // Create a DrawingBrush and use it to 
            // paint the rectangle.
            DrawingBrush myBrush = new DrawingBrush();

            GeometryDrawing backgroundSquare =
                new GeometryDrawing(
                    Brushes.White,
                    null,
                    new RectangleGeometry(new Rect(0, 0, DisplayImage.Width, DisplayImage.Height)));

            GeometryGroup aGeometryGroup = new GeometryGroup();

            int columnIndex = 0;
            for (int i = 0; i < rows; i++)
            {
                int rowIndex = 0;
                for (int j = 0; j < columns; j++)
                {
                    string color = _manager.getColorName(i, j);
                    if (color == "black")
                    {
                        aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(rowIndex, columnIndex, cellWidth - 1, cellHeight - 1)));
                    }
                    rowIndex += cellWidth;
                }
                columnIndex += cellHeight;
            }

            LinearGradientBrush checkerBrush = new LinearGradientBrush();
            checkerBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));

            GeometryDrawing checkers = new GeometryDrawing(checkerBrush, null, aGeometryGroup);

            DrawingGroup checkersDrawingGroup = new DrawingGroup();
            checkersDrawingGroup.Children.Add(backgroundSquare);
            checkersDrawingGroup.Children.Add(checkers);

            myBrush.Drawing = checkersDrawingGroup;
            myBrush.Viewport = new Rect(0, 0, 1, 1);
            myBrush.TileMode = TileMode.Tile;

            exampleRectangle.Fill = myBrush;

            DisplayImage.Children.Add(exampleRectangle);
            //DisplayImage.
            //DisplayImage.ShowGridLines = true;
        }
    }
}
