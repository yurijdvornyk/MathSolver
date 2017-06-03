using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class Chart3dPoint : Chart2dPoint
    {
        public double Z { get; private set; }
        public override int Dimensions { get { return 3; } }

        public Chart3dPoint(double x, double y, double z) : base(x, y)
        {
            Z = z;
        }

        public override List<double> GetValues()
        {
            return new List<double>() { X, Y, Z };
        }
    }
}