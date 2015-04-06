using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTree
{
    class MatrixManager
    {
        public static int [,] CloneData(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            int[,] cloneMatrix = new Int32[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++ )
                {
                    cloneMatrix[i, j] = matrix[i, j];
                }

            return cloneMatrix;
        }

        public static bool CompareMetrices(int[,] matrix1, int[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int columns = matrix1.GetLength(1);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                    {
                        return false;
                    }
                }

            return true;
        }
    }
}
