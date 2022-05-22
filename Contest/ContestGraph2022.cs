using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contest
{
    public static class ContestGraph2022
    {
        public static void RunA(TextReader THR_IN, TextWriter THR_OUT)
        {
            string[] input = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int CountStates = int.Parse(input[0]);
            int CountStreets = int.Parse(input[1]);

            int[,] Streets = new int[CountStates, CountStates];

            for (int i = 0; i < CountStates; i++)
            {
                Streets[i, i] = 1;
            }

            for (int i = 0; i < CountStreets; i++)
            {
                string[] newStreet = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int stateFrom = int.Parse(newStreet[0]);
                int stateTo = int.Parse(newStreet[1]);

                Streets[stateFrom - 1, stateTo - 1] = 1;
            }

            THR_IN.Close();

            int inf = 1000;
            for (int i = 0; i < CountStates; i++)
            {
                for (int j = i; j < CountStates; j++)
                {
                    if (i == j)
                    {
                        Streets[i, j] = 0;
                    }
                    else if (Streets[i, j] == 1 && Streets[j, i] == 0)
                    {
                        Streets[i, j] = 0; Streets[j, i] = 1;
                    }
                    else if (Streets[i, j] == 0 && Streets[j, i] == 1)
                    {
                        Streets[i, j] = 1; Streets[j, i] = 0;
                    }
                    else if (Streets[i, j] == 0 && Streets[j, i] == 0)
                    {
                        Streets[i, j] = inf; Streets[j, i] = inf;
                    }
                    else if (Streets[i, j] == 1 && Streets[i, j] == 1)
                    {
                        Streets[i, j] = 0; Streets[j, i] = 0;
                    }
                }
            }

            for (int k = 0; k < CountStates; k++)
            {
                for (int i = 0; i < CountStates; i++)
                {
                    for (int j = 0; j < CountStates; j++)
                    {
                        if (Streets[i, k] + Streets[k, j] < Streets[i, j])
                        {
                            Streets[i, j] = Streets[i, k] + Streets[k, j];
                        }
                    }
                }
            }

            int answer = Streets.Cast<int>().Max();
            THR_OUT.WriteLine(answer);
            THR_OUT.Close();
        }

        public static void RunB(TextReader THR_IN, TextWriter THR_OUT)
        {
            int RoomsCount = int.Parse(THR_IN.ReadLine());
            var RoomsString = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] Rooms = new int[RoomsCount];
            bool[] RoomsWithInput = new bool[RoomsCount];
            
            for (int i = 0; i < RoomsCount; i++)
            {
                Rooms[i] = int.Parse(RoomsString[i]) - 1;
                RoomsWithInput[Rooms[i]] = true;
            }

            int RoomWithoutInput = -1;
            for (int i = 0; i < RoomsCount; i++)
            {
                if (RoomsWithInput[i] == false)
                {
                    RoomWithoutInput = i;
                    break;
                }
            }

            if (RoomWithoutInput == -1)
            {
                THR_OUT.WriteLine("-1 -1");
                return;
            }

            bool[] Input = new bool[RoomsCount];
            int RoomFrom = -1; 
            int RoomTo = RoomWithoutInput;
            for (int i = 0; i < RoomsCount; i++)
            {
                if (Input[RoomWithoutInput])
                {
                    THR_OUT.WriteLine("-1 -1");
                    return;
                }

                Input[RoomWithoutInput] = true;
                RoomFrom = RoomWithoutInput;
                RoomWithoutInput = Rooms[RoomWithoutInput];
            }

            THR_OUT.WriteLine((RoomFrom + 1) + " " + (RoomTo + 1));
        }

        public static void RunC(TextReader THR_IN, TextWriter THR_OUT)
        {
            string[] input = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int CountVertexes = int.Parse(input[0]);
            int CountEdges = int.Parse(input[1]);

            Graph graph = new Graph(CountVertexes, CountEdges);
            for (int i = 0; i < CountEdges; i++)
            {
                int[] edge = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToArray();
                graph.AddEdge(edge[0] - 1, edge[1] - 1, edge[2]);
            }
            THR_IN.Close();

            var Cycle = BellmanFord(graph);
            if (Cycle.Count == 0)
            {
                THR_OUT.WriteLine(-1);
            }  else
            {
                THR_OUT.WriteLine(1);
                THR_OUT.WriteLine(Cycle.Count - 1);
                foreach (var CycleVertex in Cycle)
                    THR_OUT.Write(CycleVertex + " ");
            }
            THR_OUT.Close();
        }

        public static List<int> BellmanFord(Graph graph)
        {
            int CountVertexes = graph.CountVertex, CountEdges = graph.CountEdge;
            int[] Weights = new int[CountVertexes];
            var CycleVertexes = Enumerable.Repeat(-1, CountVertexes).ToArray();
            List<int> CyclePath = new List<int>();

            int CycleStartVertex = -1;
            for (int i = 0; i < CountVertexes; i++)
            {
                CycleStartVertex = -1;
                for (int j = 0; j < CountEdges; j++)
                {
                    if (Weights[graph.Edges[j].End] > Weights[graph.Edges[j].Start] + graph.Edges[j].Weight)
                    {
                        Weights[graph.Edges[j].End] = Math.Max(-10000, Weights[graph.Edges[j].Start] + graph.Edges[j].Weight);
                        CycleVertexes[graph.Edges[j].End] = graph.Edges[j].Start;
                        CycleStartVertex = graph.Edges[j].End;
                    }
                }
            }

            if (CycleStartVertex != -1)
            {
                int CycleEndVertex = CycleStartVertex;
                for (int i = 0; i < CountVertexes; i++)
                {
                    CycleEndVertex = CycleVertexes[CycleEndVertex];
                }

                while (CyclePath.Count() < 2 || CycleStartVertex != CycleEndVertex)
                {
                    CyclePath.Add(CycleStartVertex + 1);
                    CycleStartVertex = CycleVertexes[CycleStartVertex];
                }

                CyclePath.Reverse();
                CyclePath.Add(CyclePath[0]);
            }

            return CyclePath;
        }
        public static void RunD(TextReader THR_IN, TextWriter THR_OUT)
        {
            string[] input = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int CountVertexes = int.Parse(input[0]);
            int CountEdges = int.Parse(input[1]);
            Graph graph = new Graph(CountVertexes, CountEdges);

            for (int i = 0; i < CountEdges; i++)
            {
                int[] edge = THR_IN.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToArray();
                graph.AddEdge(edge[0] - 1, edge[1] - 1, edge[2]);
            }

            THR_IN.Close();

            BreadthFirstSearch bfs = new BreadthFirstSearch(graph);

            int MaxFlow = 0;
            int[][] AdjMatrix = new int[CountVertexes][];
            for (int i = 0; i < CountVertexes; i++)
            {
                AdjMatrix[i] = new int[CountVertexes];
                for (int j = 0; j < CountVertexes; j++)
                {
                    AdjMatrix[i][j] = graph.AdjacencyMatrix[i][j];
                }
            }

            bool pathExist = true;
            while (true)
            {
                bfs.Start(AdjMatrix, 0, CountVertexes - 1);

                int min = bfs.BFSPath.Min();

                for (int i = 1; i < bfs.BFSOrder.Count; i++)
                {
                    AdjMatrix[bfs.BFSOrder[i - 1]][bfs.BFSOrder[i]] -= min;
                    if (AdjMatrix[bfs.BFSOrder[i - 1]][bfs.BFSOrder[i]] < 0)
                        pathExist = false;
                }

                MaxFlow += min;

                if (!pathExist)
                    break;
            }

            THR_OUT.WriteLine(MaxFlow);
            THR_OUT.Close();
        }
        public class Edge : IComparable<Edge>
        {
            #region Fields
            public int Start;
            public int End;
            public int Weight;
            #endregion

            #region Methods
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
            #endregion

            #region Constructors
            public Edge() { }
            public Edge(int Start, int End, int Weight)
            {
                this.Start = Start;
                this.End = End;
                this.Weight = Weight;
            }
            #endregion
        }

        class CycleGraphException : Exception
        {
            public CycleGraphException(string message)
                : base(message) { }
            public CycleGraphException()
                : base() { }
        }
        class IntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x.CompareTo(y);
            }
        }
        class IntComparerOrder : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y.CompareTo(x);
            }
        }
        public class Graph : ICloneable
        {
            #region Fields
            public int CountVertex;
            public int CountEdge;

            public int[][] AdjacencyMatrix;
            public Dictionary<int, List<int>> AdjacencyList;
            public List<Edge> Edges;
            #endregion

            #region Methods
            // ВХОД: Для списка смежности
            public void AddEdge(int HostVertex, IEnumerable<int> OutVertex)
            {
                // Заполняем матрицу смежности
                foreach (var Vertex in OutVertex)
                {
                    // Балансировка рёбер
                    int _Vertex = Vertex;
                    int _HostVertex = HostVertex;
                    if (_Vertex > _HostVertex)
                    {
                        int tmp = _HostVertex;
                        _HostVertex = _Vertex;
                        _Vertex = tmp;
                    }

                    // Заполянем список рёбер
                    Edges.Add(new Edge(_HostVertex, _Vertex, 1));

                    AdjacencyMatrix[HostVertex][Vertex] = 1;
                    AdjacencyMatrix[Vertex][HostVertex] = 1;
                }
                // Заполняем список смежности
                if (AdjacencyList.ContainsKey(HostVertex))
                    AdjacencyList[HostVertex] = new List<int>(OutVertex);
                else
                    AdjacencyList.Add(HostVertex, new List<int>(OutVertex));
            }
            // ВХОД: Для матрицы смежности
            public void AddEdge(int FirstVertex, int SecondVertex, int Weight)
            {
                // Балансировка рёбер
                //if (FirstVertex > SecondVertex)
                //{
                //    int tmp = SecondVertex;
                //    SecondVertex = FirstVertex;
                //    FirstVertex = tmp;
                //}

                // Заполянем список рёбер
                Edges.Add(new Edge(FirstVertex, SecondVertex, Weight));

                // Заполяняем матрицу смежности
                AdjacencyMatrix[FirstVertex][SecondVertex] = Weight;
                AdjacencyMatrix[SecondVertex][FirstVertex] = Weight;

                // Заполянем список смежности
                if (AdjacencyList.ContainsKey(FirstVertex))
                    AdjacencyList[FirstVertex].Add(SecondVertex);
                else
                    AdjacencyList.Add(FirstVertex, new List<int> { FirstVertex });

                if (AdjacencyList.ContainsKey(SecondVertex))
                    AdjacencyList[SecondVertex].Add(FirstVertex);
                else
                    AdjacencyList.Add(SecondVertex, new List<int> { FirstVertex });
            }
            public void Sort()
            {
                Edges.Sort();
            }

            public object Clone()
            {
                return new Graph(this.CountVertex, this.CountEdge)
                {
                    AdjacencyList = this.AdjacencyList,
                    AdjacencyMatrix = this.AdjacencyMatrix,
                    Edges = this.Edges
                };
            }
            #endregion Methods

            #region Constructors
            public Graph(int CountVertexes, int CountEdges)
            {
                CountEdge = CountEdges;
                CountVertex = CountVertexes;

                AdjacencyMatrix = new int[CountVertex][];
                for (int adjVertexString = 0; adjVertexString < CountVertex; adjVertexString++)
                    AdjacencyMatrix[adjVertexString] = new int[CountVertex];

                AdjacencyList = new Dictionary<int, List<int>>();

                Edges = new List<Edge>();
            }
            #endregion Constructors
        }
        public class DepthFirstSearch
        {
            #region Fields
            public List<int> DFSOrder;
            public Graph Graph;
            public HashSet<int> VisitedVertexes;
            #endregion Fields

            #region Methods
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
            #endregion Methods

            #region Constructors
            public DepthFirstSearch(Graph graph)
            {
                Graph = graph;
                DFSOrder = new List<int>();
                VisitedVertexes = new HashSet<int>();
            }
            #endregion Constructors
        }
        public class BreadthFirstSearch
        {
            #region Fields
            public List<int> BFSOrder;
            public List<int> BFSPath;
            public Graph Graph;
            #endregion

            #region Methods
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
            #endregion Methods

            #region Constructors
            public BreadthFirstSearch(Graph graph)
            {
                Graph = graph;
                BFSOrder = new List<int>();
                BFSPath = new List<int>();
            }
            #endregion Constructors
        }
    }
}
