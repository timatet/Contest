using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    class EdgeComparer : IComparer<Edge>
    {
        public int Compare(Edge x, Edge y)
        {
            return x.Start.CompareTo(y.Start);
        }
    }
}
