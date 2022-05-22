using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class Edge : IComparable<Edge>
    {
        public int Start;
        public int End;
        public int Weight;

        public int CompareTo(Edge other)
        {
            if (this.Weight > other.Weight)
            {
                return 1;
            }
            else if (this.Weight < other.Weight)
            {
                return -1;
            }
            else
            {
                if (this.Start > other.Start)
                {
                    return 1;
                }
                else if (this.Start < other.Start)
                {
                    return -1;
                }
                else return 0;
            }
        }

        public void SwapVertexes()
        {
            int tmp = Start;
            Start = End;
            End = tmp;
        }

        public override string ToString()
        {
            return $"{this.Start} {this.End}";
        }

        public bool Incident(int otherVertex)
        {
            if (this.Start == otherVertex || this.End == otherVertex)
                return true;
            return false;
        }
        public int GetOtherIncident(int otherVertex)
        {
            if (this.Start == otherVertex)
            {
                return this.End;
            }
            else if (this.End == otherVertex)
            {
                return this.Start;
            }
            else return -1;
        }
        public Edge() { }
        public Edge(int Start, int End, int Weight)
        {
            this.Start = Start;
            this.End = End;
            this.Weight = Weight;
        }
    }
}
