using System.Collections.Generic;
using System.Text;

namespace ProblemSdk.Result
{
    public abstract class ChartPoint : IChartPoint
    {
        public override bool Equals(object obj)
        {
            if (GetType().Equals(obj.GetType()))
            {
                List<double> thisValues = GetValues();
                List<double> otherValues = (obj as IChartPoint).GetValues();
                if (thisValues.Count != otherValues.Count)
                {
                    return false;
                }
                for (int i = 0; i < thisValues.Count; ++i)
                {
                    if (thisValues[i] != otherValues[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            List<double> values = GetValues();
            result.Append("(");
            for (int i = 0; i < values.Count; ++i)
            {
                result.Append(values[i].ToString());
                if (i < values.Count - 1)
                {
                    result.Append(", ");
                }
            }
            result.Append(")");
            return result.ToString();
        }

        public abstract int Dimensions { get; }

        public abstract List<double> GetValues();
    }
}
