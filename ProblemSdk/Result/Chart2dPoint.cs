using System;
using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class Chart2dPoint : ChartPoint
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public override int Dimensions { get { return 2; } }

        public Chart2dPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override List<double> GetValues()
        {
            return new List<double>() { X, Y };
        }
    }
}