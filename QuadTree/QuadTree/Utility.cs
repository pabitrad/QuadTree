using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace QuadTree
{
    class Utility
    {
        public static void SaveWindow(Grid window, int dpi, string filename)
        {
            var rtb = new RenderTargetBitmap(
                            (int)window.Width, //width
                            (int)window.Width, //height
                            dpi, //dpi x
                            dpi, //dpi y
                            PixelFormats.Pbgra32 // pixelformat
                );

            rtb.Render(window);
            SaveRTBAsPNG(rtb, filename);
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }


        public static void CreateBitmapFromVisual(Visual target, string filename)
        {
            if (target == null)
                return;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

            RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            PngBitmapEncoder png = new PngBitmapEncoder();

            png.Frames.Add(BitmapFrame.Create(rtb));

            using (Stream stm = File.Create(filename))
            {
                png.Save(stm);
            }
        }

        public static void SaveText(string text, string filename)
        {
            File.WriteAllText(filename, text);
        }

        public static void SaveMatrix(int[,] matrix, string fileName)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            string strMatrix = rows.ToString() + " " + columns.ToString() + " 1 0" + Environment.NewLine;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    strMatrix += (matrix[i, j] + " ");
                }
                strMatrix += Environment.NewLine;
            }

            File.WriteAllText(fileName, strMatrix);
        }
    }
}
