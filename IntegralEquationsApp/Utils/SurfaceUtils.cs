using ProblemSdk.Result;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace IntegralEquationsApp.Utils
{
    public class SurfaceUtils
    {
        /// <summary>
        /// Returns matrix of the Z values for given collection of 3D points
        /// </summary>
        /// <param name="points">Chart3dPoint collection</param>
        /// <returns>Returns matrix of the Z values</returns>
        public static double[,] GetValueMatrixFromPoints(List<Chart3dPoint> points)
        {
            // TODO: Refactor the code
            SortedDictionary<double, int> xIndexes = new SortedDictionary<double, int>();
            SortedDictionary<double, int> yIndexes = new SortedDictionary<double, int>();
            points.ForEach(point =>
            {
                if (!xIndexes.ContainsKey(point.X))
                {
                    int index = xIndexes.Count;
                    xIndexes.Add(point.X, index);
                }
                if (!yIndexes.ContainsKey(point.Y))
                {
                    int index = yIndexes.Count;
                    yIndexes.Add(point.Y, index);
                }
            });
            double[,] data = new double[xIndexes.Count, yIndexes.Count];
            points.ForEach(point =>
            {
                data[xIndexes[point.X], yIndexes[point.Y]] = point.Z;
            });
            if (data.GetLength(0) == 1)
            {
                double[,] fixedData = new double[data.GetLength(0) + 1, data.GetLength(1)];
                for (int j = 0; j < data.GetLength(1); ++j)
                {
                    fixedData[0, j] = data[0, j];
                    fixedData[1, j] = fixedData[0, j];
                }
                data = fixedData;
            }
            if (data.GetLength(1) == 1)
            {
                double[,] fixedData = new double[data.GetLength(0), data.GetLength(1) + 1];
                for (int i = 0; i < data.GetLength(1); ++i)
                {
                    fixedData[i, 0] = data[i, 0];
                    fixedData[i, 1] = fixedData[i, 0];
                }
                data = fixedData;
            }
            return data;
        }
    }
}
