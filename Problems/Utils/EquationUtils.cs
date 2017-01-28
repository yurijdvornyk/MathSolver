using System.IO;

namespace Problems.Utils
{
    public class EquationUtils
    {
        public static readonly string EQUATIONS_PATH = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Problems", "Equations");

        public static string GetPathForCurrentProblemEquation<T>(T type)
        {
            return Path.Combine(EQUATIONS_PATH, typeof(T).Name + ".png");
        }
    }
}
