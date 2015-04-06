using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.Win32;
using System.Windows;

namespace QuadTree
{
    class QuadTreeManager
    {
        private int[,] _points = null;
        
        private int _rows = 0;
        private int _columns = 0;
        private QuadTreeImp _quadTree;

        private string _preOrderText;

        public int[,] Points
        {
            get
            {
                return _points;
            }
        }

        public int Rows
        {
            get { return _rows; }
        }

        public int Columns
        {
            get { return _columns; }
        }

        public QuadTreeImp QuadTree
        {
            get
            {
                return _quadTree;
            }
        }

        public string getColorName(int row, int column)
        {
            string colourName = string.Empty;
            if (_points[row, column] == 1)
            {
                colourName = "black";
            }
            else
            {
                colourName = "white";
            }

            return colourName;
        }

        public void drwaQuadTree(Canvas canvas)
        {
            _quadTree = new QuadTreeImp(_points, _rows, _columns, 0, Direction.ROOT, canvas);
            _quadTree = _quadTree.createQuardTree();

            _quadTree.Position = getRootNodePosition(canvas);
            _quadTree.draw(canvas);
        }

        public void drwaQuadTree(Canvas canvas, string preOrderTextFile)
        {
            StreamReader fileReader = File.OpenText(preOrderTextFile);
            string header = fileReader.ReadLine();
            char[] delimiterChars = { ' ' };
            string[] headerEntries = header.Split(delimiterChars);

            _rows = Convert.ToInt32(headerEntries[0]);
            _columns = Convert.ToInt32(headerEntries[1]);

            _preOrderText = fileReader.ReadLine();
            _quadTree = new QuadTreeImp(_rows, _columns, 0, Direction.ROOT, canvas);
            _quadTree = _quadTree.createQuardTree(ref _preOrderText);

            _quadTree.Position = getRootNodePosition(canvas);
            _quadTree.draw(canvas);
        }

        public void loadData(string fileName)
        {
            StreamReader fileReader = File.OpenText(fileName);
            if (fileReader != null)
            {
                string header = fileReader.ReadLine();
                char[] delimiterChars = { ' ' };
                string[] headerEntries = header.Split(delimiterChars);
                _rows = Convert.ToInt32(headerEntries[0]);
                _columns = Convert.ToInt32(headerEntries[1]);
                int maxValue = Convert.ToInt32(headerEntries[2]);
                int minValue = Convert.ToInt32(headerEntries[3]);

                _points = new int[_rows, _columns];

                string line = fileReader.ReadLine();
                int rowIndex = 0;

                while (line.Length > 0) 
                {
                    string[] lineEntries = line.Split(delimiterChars);
                    for (int i=0; i<_columns; i++)
                    {
                        _points[rowIndex, i] = Convert.ToInt32(lineEntries[i]);
                    }
                    rowIndex++;
                    line = fileReader.ReadLine();
                }
            }
        }

        public bool IsDataLoaded()
        {
            if (_points != null)
            {
                return true;
            }

            return false;
        }

        public void cleraData()
        {
            _points = null;
            _rows = 0;
            _columns = 0;
        }

        private Point getRootNodePosition(Canvas canvas)
        {
            return new Point(canvas.Width / 2, 20);
        }

        public void clearQuadTree()
        {
            _quadTree = null;
        }

        public string generatePreOrderText()
        {
            _preOrderText = _rows.ToString() + " " + _columns.ToString() + " 1 0" + Environment.NewLine; //Add row and column dimension
            if (_quadTree != null)
            {
                addToPreOrderText(_quadTree);
            }

            return _preOrderText;
        }

        public void convertQuadTreeToMatrix()
        {
            _quadTree.computeMatrix();
            _points = _quadTree.Points;
            _rows = _quadTree.Rows;
            _columns = _quadTree.Columns;
        }

        private void addToPreOrderText(QuadTreeImp quadTree)
        {
            if (quadTree != null)
            {
                if (quadTree.RootColor == Colors.Gray)
                {
                    _preOrderText += "2 ";
                }
                else if (quadTree.RootColor == Colors.Black)
                {
                    _preOrderText += "1 ";
                }
                else if (quadTree.RootColor == Colors.White)
                {
                    _preOrderText += "0 ";
                }

                if (quadTree.NorthWest != null)
                {
                    addToPreOrderText(quadTree.NorthWest);
                }

                if (quadTree.SouthWest != null)
                {
                    addToPreOrderText(quadTree.SouthWest);
                }

                if (quadTree.SouthEast != null)
                {
                    addToPreOrderText(quadTree.SouthEast);
                }

                if (quadTree.NorthEast != null)
                {
                    addToPreOrderText(quadTree.NorthEast);
                }
            }
        }
    }
}
