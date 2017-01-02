namespace ProblemSdk.Result
{
    public class Point
    {
        private const string POINT_STRING_FORMAT = "({0}; {1})";

        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                Point other = obj as Point;
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