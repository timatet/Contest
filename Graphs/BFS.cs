using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class BreadthFirstSearch
    {
        public List<int> BFSOrder;
        public List<int> BFSPath;
        public Graph Graph;

        public bool Start(int StartVertex)
        {
            return Start(Graph.AdjacencyMatrix, StartVertex, -1);
        }
        public bool Start(int StartVertex, int EndVertex)
        {
            return Start(Graph.AdjacencyMatrix, StartVertex, EndVertex);
        }
        public bool Start(int[][] adj, int StartVertex, int EndVertex)
        {
            BFSOrder.Clear(); BFSPath.Clear();

            bool[] VisitedVertexes = new bool[Graph.CountVertex];
            VisitedVertexes[StartVertex] = true;

            Queue<int> BFSqueue = new Queue<int>(new int[] { StartVertex });

            while (BFSqueue.Count > 0)
            {
                var Vertex = BFSqueue.Dequeue();
                if (BFSOrder.Count > 0)
                    BFSPath.Add(adj[Vertex][BFSOrder.Last()]);
                BFSOrder.Add(Vertex);

                for (int i = 0; i < Graph.CountVertex; i++)
                {
                    if (!VisitedVertexes[i] && adj[Vertex][i] > 0)
                    {
                        if (i == EndVertex)
                        {
                            BFSOrder.Add(i);
                            BFSPath.Add(adj[Vertex][i]);
                            return true;
                        }

                        BFSqueue.Enqueue(i);
                        VisitedVertexes[i] = true;
                    }
                }
            }

            return false;
        }
        public BreadthFirstSearch(Graph graph)
        {
            Graph = graph;
            BFSOrder = new List<int>();
            BFSPath = new List<int>();
        }
    }
}
