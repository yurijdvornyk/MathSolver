using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSdk.Error
{
    public class InputDataException : ProblemException
    {
        public InputDataException(string header, string message) :
            base(string.Format(Messages.INPUT_DATA_EXCEPTION, header), message)
        { }
    }
}
