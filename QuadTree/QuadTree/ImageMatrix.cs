using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QuadTree
{
    public class ImageMatrix
    {
        private int[,] _points;
        private int _rows;
        private int _columns;
        private string _preOrderText = string.Empty;

        public ImageMatrix(int[,] points, int rows, int columns)
        {
            _points = points;
            _rows = rows;
            _columns = columns;
        }

        public string generatePreOrderText()
        {
            _preOrderText = _rows.ToString() + " " + _columns.ToString() + Environment.NewLine; //Add row and column dimension
            addToPreOrderText(this);

            return _preOrderText;
        }

        private void addToPreOrderText(ImageMatrix matrix)
        {
            Color color = matrix.getNodeColor();

            if (color == Colors.Gray)
            {
                _preOrderText += "2 ";
            }
            else if (color == Colors.Black)
            {
                _preOrderText += "1 ";
            }
            else if (color == Colors.White)
            {
                _preOrderText += "0 ";
            }

            if (color == Colors.Gray)
            {
                int[,] _pointsNW = matrix.getPoints(Direction.NW);
                ImageMatrix imNW = new ImageMatrix(_pointsNW, matrix._rows / 2, matrix._columns / 2);
                addToPreOrderText(imNW);

                int[,] _pointsSW = matrix.getPoints(Direction.SW);
                ImageMatrix imSW = new ImageMatrix(_pointsSW, matrix._rows / 2, matrix._columns / 2);
                addToPreOrderText(imSW);

                int[,] _pointsSE = matrix.getPoints(Direction.SE);
                ImageMatrix imSE = new ImageMatrix(_pointsSE, matrix._rows / 2, matrix._columns / 2);
                addToPreOrderText(imSE);

                int[,] _pointsNE = matrix.getPoints(Direction.NE);
                ImageMatrix imNE = new ImageMatrix(_pointsNE, matrix._rows / 2, matrix._columns / 2);
                addToPreOrderText(imNE);
            }
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

    }
}
