namespace crickinfo_mvc_ef_core.Models.Algorithms
{
    using System;
    using System.Collections.Generic;

    public class MaxFlowAlgorithm
    {
        // 2D matrix to represent the graph
        private List<List<int>> graph;

        // Constructor to initialize the graph matrix
        public MaxFlowAlgorithm(List<List<int>> initialGraph)
        {
            // Initialize the graph with the provided input matrix
            graph = initialGraph;
        }

        // Perform BFS to find an augmenting path from source (s) to sink (t)
        public bool Bfs(int s, int t, List<int> parent)
        {
            int n = graph.Count;
            var visited = new bool[n];
            var queue = new Queue<int>();
            queue.Enqueue(s);

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < n; ++v)
                {
                    if (graph[u][v] > 0 && !visited[v])
                    {
                        parent[v] = u;
                        if (v == t)
                        {
                            return true;  // Found a path to the sink
                        }
                        visited[v] = true;
                        queue.Enqueue(v);
                    }
                }
            }
            return false;
        }

        // Find the minimum capacity (bottleneck) in the path found
        public int FindBottleNeck(int s, int t, List<int> parent)
        {
            int minFlow = int.MaxValue;
            for (int v = t; v != s; v = parent[v])
            {
                int u = parent[v];
                minFlow = Math.Min(minFlow, graph[u][v]);
            }
            return minFlow;
        }

        // Update the residual graph after augmenting the flow
        public void CreateResidualGraph(int s, int t, List<int> parent, int flow)
        {
            var path = new Stack<int>();
            for (int v = t; v != s; v = parent[v])
            {
                path.Push(v);
                int u = parent[v];
                graph[u][v] -= flow;
                graph[v][u] += flow;
            }
            Console.Write(s);
            while (path.Count > 0)
            {
                Console.Write(" -> " + path.Pop());
            }
            Console.WriteLine(" = " + flow);
        }

        // Max Flow function to find the maximum flow in the graph
        public int MaxFlow(int source, int sink)
        {
            int maxFlow = 0;
            int n = graph.Count;

            while (true)
            {
                var parent = new List<int>(new int[n]);
                for (int i = 0; i < n; i++) parent[i] = -1;

                if (Bfs(source, sink, parent))
                {
                    int flow = FindBottleNeck(source, sink, parent);
                    maxFlow += flow;
                    CreateResidualGraph(source, sink, parent, flow);
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("MAX FLOW : " + maxFlow);
                
            return maxFlow;
        }
    }

}
