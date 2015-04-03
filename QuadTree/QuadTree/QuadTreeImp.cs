using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

using Petzold.Media2D;

namespace QuadTree
{
    /// <summary>
    /// http://en.wikipedia.org/wiki/Quadtree
    /// </summary>
    /// 

    public enum Direction : int
    {
        ROOT,
        NW,
        NE,
        SW,
        SE
    }

    class QuadTreeImp
    {
        const int QT_NODE_CAPACITY = 4;
        private int[,] _points = null;
        private int _rows;
        private int _columns;
        private int _depth;
        private Point _position;
        private Direction _rootDirection;

        private int NODE_DIMENSION = 15;

        Color _rootColor;
        Canvas _canvas;

        QuadTreeImp _northWest = null;
        QuadTreeImp _northEast = null;
        QuadTreeImp _southWest = null;
        QuadTreeImp _southEast = null;

        public QuadTreeImp NorthWest
        {
            get
            {
                return _northWest;
            }
        }

        public QuadTreeImp NorthEast
        {
            get
            {
                return _northEast;
            }
        }

        public QuadTreeImp SouthWest
        {
            get
            {
                return _southWest;
            }
        }

        public QuadTreeImp SouthEast
        {
            get
            {
                return _southEast;
            }
        }

        public Color RootColor
        {
            get
            {
                return _rootColor;
            }
        }

        public int[,] Points
        {
            get
            {
                return _points;
            }
        }

        public int Rows
        {
            get
            {
                return _rows;
            }
        }

        public int Columns
        {
            get
            {
                return _columns;
            }
        }

        public QuadTreeImp(int[,] points, int rows, int columns, int depth, Direction direction, Canvas canvas)
        {
            _points = points;
            _rows = rows;
            _columns = columns;
            _depth = depth;
            _rootDirection = direction;
            _canvas = canvas;
        }

        public QuadTreeImp(int rows, int columns, int depth, Direction direction, Canvas canvas)
        {
            _rows = rows;
            _columns = columns;
            _depth = depth;
            _rootDirection = direction;
            _canvas = canvas;
        }
		
        public Point Position
        {
            set 
            {
                _position = value;
            }
        }

        public QuadTreeImp createQuardTree()
        {
            if (_points == null || _rows == 0 || _columns == 0)
            {
                return null;
            }
            _rootColor = getNodeColor();

            if (_rootColor == Colors.Gray)
            {
                int[,] _pointsNW = getPoints(Direction.NW);
                QuadTreeImp qtNW = new QuadTreeImp(_pointsNW, _rows/2, _columns/2, _depth + 1, Direction.NW, _canvas);
                _northWest = qtNW.createQuardTree();

                int[,] _pointsSW = getPoints(Direction.SW);
                QuadTreeImp qtSW = new QuadTreeImp(_pointsSW, _rows / 2, _columns / 2, _depth + 1, Direction.SW, _canvas);
                _southWest = qtSW.createQuardTree();

                int[,] _pointsSE = getPoints(Direction.SE);
                QuadTreeImp qtSE = new QuadTreeImp(_pointsSE, _rows / 2, _columns / 2, _depth + 1, Direction.SE, _canvas);
                _southEast = qtSE.createQuardTree();

                int[,] _pointsNE = getPoints(Direction.NE);
                QuadTreeImp qtNE = new QuadTreeImp(_pointsNE, _rows / 2, _columns / 2, _depth + 1, Direction.NE, _canvas);
                _northEast = qtNE.createQuardTree();
            }

            return this;
        }

        public QuadTreeImp createQuardTree(ref string preOrderText)
        {
            _rootColor = getNodeColor(ref preOrderText);

            if (_rootColor == Colors.Gray)
            {
                QuadTreeImp qtNW = new QuadTreeImp(_rows / 2, _columns / 2, _depth + 1, Direction.NW, _canvas);
                _northWest = qtNW.createQuardTree(ref preOrderText);

                QuadTreeImp qtSW = new QuadTreeImp(_rows / 2, _columns / 2, _depth + 1, Direction.SW, _canvas);
                _southWest = qtSW.createQuardTree(ref preOrderText);

                QuadTreeImp qtSE = new QuadTreeImp(_rows / 2, _columns / 2, _depth + 1, Direction.SE, _canvas);
                _southEast = qtSE.createQuardTree(ref preOrderText);

                QuadTreeImp qtNE = new QuadTreeImp(_rows / 2, _columns / 2, _depth + 1, Direction.NE, _canvas);
                _northEast = qtNE.createQuardTree(ref preOrderText);
			}
			
			return this;
		}
		
        private int[,] getPoints(Direction direction)
        {
            int rows = _rows / 2;
            int columns = _columns / 2;

            int[,] points = new int[rows, columns];

            // For direction NW
            int startRowIndex = 0;
            int startColumnIndex = 0;

            switch (direction)
            {
                case Direction.SW:
                    startRowIndex = rows;
                    break;

                case Direction.NE:
                    startColumnIndex = columns;
                    break;

                case Direction.SE:
                    startRowIndex = rows;
                    startColumnIndex = columns;
                    break;
            }

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    points[i, j] = _points[startRowIndex + i, startColumnIndex + j];
                }

            return points;
        }

        private Color getNodeColor()
        {
            int firstPoint = _points[0, 0];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (firstPoint != _points[i, j])
                    {
                        return Colors.Gray;
                    }
                }
            }

            if (firstPoint == 0)
            {
                return Colors.White;
            }

            return Colors.Black;
        }

        private Color getNodeColor(ref string preOrderText)
		{
            preOrderText = preOrderText.Trim();

            string subStr = preOrderText.Substring(0, 1);
            preOrderText = preOrderText.Remove(0, 1);

            int color = Convert.ToInt32(subStr);
			if (color == 2)
			{
				return Colors.Gray;
			}
			else if (color == 1)
			{
				return Colors.Black;
			}
			else if (color == 0)
			{
				return Colors.White;
			}
			
			return Colors.White;
		}
		
        public void draw(Canvas canvas)
        {
            drawNode(canvas, _rootColor);

            if (_northWest != null)
            {
                _northWest.setPosition(Direction.NW, _position);// set position of child
                _northWest.draw(canvas);
            }

            if (_southWest != null)
            {
                _southWest.setPosition(Direction.SW, _position);// set position of child
                _southWest.draw(canvas);
            }

            if (_southEast != null)
            {
                _southEast.setPosition(Direction.SE, _position);
                _southEast.draw(canvas);
            }

            if (_northEast != null)
            {
                _northEast.setPosition(Direction.NE, _position);
                _northEast.draw(canvas);
            }

            drawArrows(canvas);
        }

        private void drawNode(Canvas canvas, Color nodeColor)
        {
            Shape renderShape = new Ellipse() { Width = NODE_DIMENSION, Height = NODE_DIMENSION };
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 1);
            brush.EndPoint = new Point(1, 1);
            if (nodeColor == Colors.Black || nodeColor == Colors.White)
            {
                brush.GradientStops.Add(new GradientStop(nodeColor, 1.0));
            }
            else if (nodeColor == Colors.Gray)
            {
                brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            }
            renderShape.Fill = brush;

            Canvas.SetLeft(renderShape, _position.X);
            Canvas.SetTop(renderShape, _position.Y);
            canvas.Children.Add(renderShape);
        }

        void drawArrows(Canvas canvas)
        {
            Point rootPosition = _position;
            rootPosition.Y += NODE_DIMENSION;
            rootPosition.X += NODE_DIMENSION / 2;

            Point childPosition;
            if (_northWest != null)
            {
                childPosition = _northWest._position;
                drawArrow(canvas, rootPosition, childPosition);
            }

            if (_southWest != null)
            {
                childPosition = _southWest._position;
                drawArrow(canvas, rootPosition, childPosition);
            }

            if (_southEast != null)
            {
                childPosition = _southEast._position;
                drawArrow(canvas, rootPosition, childPosition);
            }

            if (_northEast != null)
            {
                childPosition = _northEast._position;
                drawArrow(canvas, rootPosition, childPosition);
            }
        }

        void drawArrow(Canvas canvas, Point fromPosition, Point toPosition)
        {
            ArrowLine arrow = new ArrowLine() { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = 1.5 };
            arrow.X1 = fromPosition.X;
            arrow.Y1 = fromPosition.Y;

            arrow.X2 = toPosition.X + NODE_DIMENSION/2;
            arrow.Y2 = toPosition.Y;

            canvas.Children.Add(arrow);
        }

        void setPosition(Direction direction, Point nodePosition)
        {
            _position = new Point();
            _position.Y = nodePosition.Y +_depth * 50;
            _position.X = nodePosition.X;

            if (_depth > 0)
            {
                double nodeWidth = _canvas.Width / Math.Pow(QT_NODE_CAPACITY, _depth);
                switch (direction)
                {
                    case Direction.NW:
                        _position.X = nodePosition.X - (nodeWidth * 1.5);
                        break;

                    case Direction.SW:
                        _position.X = nodePosition.X - (nodeWidth * 0.5);
                        break;

                    case Direction.SE:
                        _position.X = nodePosition.X + (nodeWidth * 0.5);
                        break;

                    case Direction.NE:
                        _position.X = nodePosition.X + (nodeWidth * 1.46);
                        break;
                }
            }
        }

        /// <summary>
        /// Compute matrix from quad tree
        /// </summary>
        public void computeMatrix()
        {
            if (NorthWest != null)
            {
                if (NorthWest.RootColor == Colors.Gray)
                {
                    NorthWest.computeMatrix();
                }
                else
                {
                    NorthWest.setMatrix(NorthWest.RootColor);
                }
            }

            if (SouthWest != null)
            {
                if (SouthWest.RootColor == Colors.Gray)
                {
                    SouthWest.computeMatrix();
                }
                else
                {
                    SouthWest.setMatrix(SouthWest.RootColor);
                }
            }

            if (SouthEast != null)
            {
                if (SouthEast.RootColor == Colors.Gray)
                {
                    SouthEast.computeMatrix();
                }
                else
                {
                    SouthEast.setMatrix(SouthEast.RootColor);
                }
            }

            if (NorthEast != null)
            {
                if (NorthEast.RootColor == Colors.Gray)
                {
                    NorthEast.computeMatrix();
                }
                else
                {
                    NorthEast.setMatrix(NorthEast.RootColor);
                }
            }

            mergeMatrices();
        }

        private void setMatrix(Color rootColor)
        {
            _points = new int[_rows, _columns];
            if (rootColor == Colors.White)
            {
                fillMatrix(0);
            }
            else if (rootColor == Colors.Black)
            {
               fillMatrix(1);
            }
        }

        private void fillMatrix(int number)
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++ )
                {
                    _points[i, j] = number;
                }
            }
        }

        private void mergeMatrices()
        {
            int rowStartIndex = 0;
            int columnStartIndex = 0;

            if (NorthWest._points != null && SouthWest._points != null && SouthEast._points != null && NorthEast._points != null)
            {
               _points = new int[_rows, _columns];

               //Add North west points
               for (int i = 0; i < _rows/2; i++)
                   for (int j = 0; j < _columns/2; j++)
                   {
                       _points[i, j] = NorthWest._points[i, j];
                   }

               columnStartIndex = _columns / 2;
               //Add North east points
               for (int i = 0; i < _rows / 2; i++)
                   for (int j = 0; j < _columns / 2; j++)
                   {
                       _points[i, columnStartIndex + j] = NorthEast._points[i, j];
                   }

               rowStartIndex = _rows / 2;
               //Add South west points
               for (int i = 0; i < _rows / 2; i++)
                   for (int j = 0; j < _columns / 2; j++)
                   {
                       _points[rowStartIndex + i, j] = SouthWest._points[i, j];
                   }

               //Add South east points
               for (int i = 0; i < _rows / 2; i++)
                   for (int j = 0; j < _columns / 2; j++)
                   {
                       _points[rowStartIndex + i, columnStartIndex + j] = SouthEast._points[i, j];
                   }
            }
        }
    }
}
