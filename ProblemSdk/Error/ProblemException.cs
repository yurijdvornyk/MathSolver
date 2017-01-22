using System;

namespace ProblemSdk.Error
{
    public class ProblemException : Exception
    {
        public ProblemException(string header, string message) : base(getFormattedMessage(header, message)) { }

        private static string getFormattedMessage(string header, string message)
        {
            return string.Format("{0}:\n{1}", header, message);
        }
    }
}
