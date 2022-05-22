using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class DepthFirstSearch
    {
        public List<int> DFSOrder;
        public Graph Graph;
        public HashSet<int> VisitedVertexes;

        public void Start(int StartVertex)
        {
            VisitedVertexes.Add(StartVertex);
            DFSOrder.Add(StartVertex);

            int Numerator = 0;
            while (VisitedVertexes.Count != Graph.CountVertex)
            {
                if (Numerator == Graph.CountVertex)
                    return;

                if (Graph.AdjacencyMatrix[StartVertex][Numerator++] != 0)
                {
                    if (!VisitedVertexes.Contains(Numerator - 1))
                    {
                        Start(Numerator - 1);
                    }
                }
            }
        }
        public DepthFirstSearch(Graph graph)
        {
            Graph = graph;
            DFSOrder = new List<int>();
            VisitedVertexes = new HashSet<int>();
        }
    }
}
