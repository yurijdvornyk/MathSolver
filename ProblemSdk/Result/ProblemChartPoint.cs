namespace ProblemSdk.Result
{
    public class ProblemChartPoint
    {
        private const string POINT_STRING_FORMAT = "({0}; {1})";

        public double X { get; set; }
        public double Y { get; set; }

        public ProblemChartPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is ProblemChartPoint)
            {
                ProblemChartPoint other = obj as ProblemChartPoint;
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(POINT_STRING_FORMAT, X, Y);
        }
    }
}