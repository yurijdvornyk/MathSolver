using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralEquationsApp.Components.InputData.ItemView
{
    public class ItemValueParseException: ArgumentException
    {
        private const string MESSAGE = "Cannot parse value in {0}.";

        public ItemValueParseException(Type type): base(string.Format(MESSAGE, type.Name)) { }
    }
}
