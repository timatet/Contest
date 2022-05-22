using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class CycleGraphException : Exception
    {
        public CycleGraphException(string message)
            : base(message) { }
        public CycleGraphException()
            : base() { }
    }
}
