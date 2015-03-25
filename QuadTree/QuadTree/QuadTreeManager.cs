using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

using Microsoft.Win32;
using System.Windows;

namespace QuadTree
{
    class QuadTreeManager
    {
        private int[,] _points = null;
        
        private int _rows = 0;
        private int _columns = 0;

        public int Rows
        {
            get { return _rows; }
        }

        public int Columns
        {
            get { return _columns; }
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
            QuadTreeImp quadTree = new QuadTreeImp(_points, _rows, _columns, 0, Direction.ROOT, canvas);
            quadTree = quadTree.createQuardTree();

            quadTree.Position = getRootNodePosition(canvas);
            quadTree.draw(canvas);
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
            //return new Point(400, 20);
            return new Point(canvas.Width / 2, 20);
        }
    }
}
