using ProblemSdk.Result;
using System;
using System.Collections.Generic;

namespace Problems.Helper
{
    public class PotentialHelper
    {
        private double potentialDeltaX;
        private double potentialDeltaY;
        private double potentialN;
        private double edge;
        private List<Tuple<double, double>> t;
        private List<double> tau;

        public PotentialHelper(double potentialDeltaX, double potentialDeltaY, double potentialN, 
            double edge, List<Tuple<double, double>> t, List<double> tau)
        {
            this.potentialDeltaX = potentialDeltaX;
            this.potentialDeltaY = potentialDeltaY;
            this.potentialN = potentialN;
            this.edge = edge;
            this.t = t;
            this.tau = tau;
        }

        public List<Chart2dPoint> getXZProjection()
        {
            List<Chart2dPoint> result = new List<Chart2dPoint>();
            double stepX = 2 * potentialDeltaX / potentialN;
            for (int i = 0; i < potentialN; ++i)
            {
                double x = edge - potentialDeltaX + i * stepX;
                result.Add(new Chart2dPoint(x, getPotentialAbsolute(x, 0)));
            }
            return result;
        }

        public List<Chart2dPoint> getYZProjection()
        {
            List<Chart2dPoint> result = new List<Chart2dPoint>();
            double stepY = 2 * potentialDeltaY / potentialN;
            for (int i = 0; i < potentialN; ++i)
            {
                double y = -potentialDeltaY + i * stepY;
                result.Add(new Chart2dPoint(y, getPotentialAbsolute(1, y)));
            }
            return result;
        }

        public List<Tuple<double, double, double>> getPotentialPoints()
        {
            List<Tuple<double, double, double>> result = new List<Tuple<double, double, double>>();
            double stepX = 2 * potentialDeltaX / potentialN;
            double stepY = 2 * potentialDeltaY / potentialN;
            for (int i = 0; i < potentialN; ++i)
            {
                double x = edge - potentialDeltaX + i * stepX;
                for (int j = 0; j < potentialN; ++j)
                {
                    double y = -potentialDeltaY + j * stepY;
                    result.Add(new Tuple<double, double, double>(x, y, getPotentialAbsolute(x, y)));
                }
            }
            return result;
        }

        private double getPotentialAbsolute(double x1, double x2)
        {
            double absolute = Math.Sqrt(Math.Pow(getDuDx1(x1, x2), 2) + Math.Pow(getDuDx2(x1, x2), 2));
            if (double.IsNaN(absolute))
            {
                absolute = 0;
            }
            return absolute;
        }

        private double getDuDx1(double x1, double x2)
        {
            double sum = 0;
            for (int i = 0; i < tau.Count; ++i)
            {
                double up = Math.Pow(t[i].Item1 - x1, 2) + Math.Pow(t[i].Item2, 2);
                double down = Math.Pow(t[i].Item1 - x1, 2) + Math.Pow(t[i].Item2, 2);
                sum += tau[i] * Math.Log(up / down);
            }
            return sum / (4 * Math.PI);
        }

        private double getDuDx2(double x1, double x2)
        {
            double sum = 0;
            for (int i = 0; i < tau.Count; ++i)
            {
                double atan1 = Math.Atan(Math.Abs(t[i].Item2 - x1) / Math.Abs(x2));
                double atan2 = Math.Atan(Math.Abs(t[i].Item1 - x1) / Math.Abs(x2));
                sum += tau[i] * (atan1 - atan2);
            }
            return sum / (2 * Math.PI);
        }
    }
}
