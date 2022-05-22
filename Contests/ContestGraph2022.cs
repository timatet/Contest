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
                    if (Streets[i, j] == 1 && Streets[j, i] == 0)
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

            var Cycle = graph.GetCycleWithBellmanFord();
            if (Cycle.Count == 0)
            {
                THR_OUT.WriteLine(-1);
            }
            else
            {
                THR_OUT.WriteLine(1);
                THR_OUT.WriteLine(Cycle.Count - 1);
                foreach (var CycleVertex in Cycle)
                    THR_OUT.Write(CycleVertex + " ");
            }
            THR_OUT.Close();
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
    }
}
