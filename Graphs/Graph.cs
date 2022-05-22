using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class Graph : ICloneable
    {
        public int CountVertex;
        public int CountEdge;

        public int[][] AdjacencyMatrix;
        public Dictionary<int, List<int>> AdjacencyList;
        public List<Edge> Edges;

        private SortedDictionary<int, List<Edge>> GroupEdgesForSorting;

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

            if (GroupEdgesForSorting.ContainsKey(Weight))
            {
                GroupEdgesForSorting[Weight].Add(new Edge(FirstVertex, SecondVertex, Weight));
            }
            else
            {
                GroupEdgesForSorting.Add(Weight, new List<Edge> { new Edge(FirstVertex, SecondVertex, Weight) });
            }
        }
        public void Sort()
        {
            Edges.Sort();
        }

        public IEnumerable<Edge> StartKruskalAlg()
        {
            //Сортируем рёбра графа в порядке возрастания их веса
            var EVertexes = from EdgeList in GroupEdgesForSorting
                                //from Edge in EdgeList.Value.OrderByDescending(t => t.Start)
                            select EdgeList.Value;

            foreach (var EdgeList in EVertexes)
            {
                EdgeList.Sort(new EdgeComparer());
            }

            var E = from EdgeList in EVertexes
                    from Edge in EdgeList
                    select Edge;

            // Дек отсортированных рёбер
            deque<Edge> StackEVertexes = new deque<Edge>(E);
            // Стек рёбер остовного дерева, здесь оно формируется
            List<Edge> SpanningTree = new List<Edge>();
            // Массив всех вершин графа, на начальной стадии каждая вершина
            // - отдельный граф
            var VertexParents = new int[CountVertex];
            for (int i = 0; i < CountVertex; i++)
            {
                VertexParents[i] = i;
            }

            for (int Edge = 0; Edge < CountEdge; Edge++)
            {
                int First = VertexParents[StackEVertexes[Edge].Start];
                int Second = VertexParents[StackEVertexes[Edge].End];
                if (First != Second)
                { //Проверка на связность
                    for (int Parent = 0; Parent < CountVertex; Parent++)
                    {
                        if (VertexParents[Parent] == First)
                            VertexParents[Parent] = Second;
                    }

                    SpanningTree.Add(StackEVertexes[Edge]);
                }
            }

            return SpanningTree;
        }

        public List<int> GetCycleWithBellmanFord()
        {
            int[] Weights = new int[CountVertex];
            var CycleVertexes = Enumerable.Repeat(-1, CountVertex).ToArray();
            List<int> CyclePath = new List<int>();

            int CycleStartVertex = -1;
            for (int i = 0; i < CountVertex; i++)
            {
                CycleStartVertex = -1;
                for (int j = 0; j < CountEdge; j++)
                {
                    if (Weights[Edges[j].End] > Weights[Edges[j].Start] + Edges[j].Weight)
                    {
                        Weights[Edges[j].End] = Math.Max(-10000, Weights[Edges[j].Start] + Edges[j].Weight);
                        CycleVertexes[Edges[j].End] = Edges[j].Start;
                        CycleStartVertex = Edges[j].End;
                    }
                }
            }

            if (CycleStartVertex != -1)
            {
                int CycleEndVertex = CycleStartVertex;
                for (int i = 0; i < CountVertex; i++)
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

        public object Clone()
        {
            return new Graph(this.CountVertex, this.CountEdge)
            {
                AdjacencyList = this.AdjacencyList,
                AdjacencyMatrix = this.AdjacencyMatrix,
                Edges = this.Edges
            };
        }
        public Graph(int CountVertexes, int CountEdges)
        {
            CountEdge = CountEdges;
            CountVertex = CountVertexes;

            AdjacencyMatrix = new int[CountVertex][];
            for (int adjVertexString = 0; adjVertexString < CountVertex; adjVertexString++)
                AdjacencyMatrix[adjVertexString] = new int[CountVertex];

            AdjacencyList = new Dictionary<int, List<int>>();

            Edges = new List<Edge>();

            GroupEdgesForSorting = new SortedDictionary<int, List<Edge>>();
        }
    }
}
