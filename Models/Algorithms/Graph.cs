using crickinfo_mvc_ef_core.Models;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace crickinfo_mvc_ef_core.Models.Algorithms
{
    public class Graph
    {

        private int[,] capacityMatrix;  // adjacency matrix to store edge capacities
        private int nodeCount;          // number of nodes in the graph
        private Dictionary<string, int> nodeIndexes;  // mapping of node names to matrix indexes

        public Graph(int maxPoints,Team t1, List<Team> teams, List<Matches> matches, List<PointsTable> pointsTable)
        {
            int teamCount = teams.Count;
            int matchCount = matches.Count;

            nodeCount = matchCount + teamCount + 2;  

            capacityMatrix = new int[nodeCount, nodeCount];
            nodeIndexes = new Dictionary<string, int>();

            nodeIndexes["s"] = 0;
            nodeIndexes["t"] = nodeCount - 1;

            int index = 1;
            if (matches.Count > 0) {
                foreach (var match in matches)
                {
                    nodeIndexes[$"match-{match.TeamA.Name}-{match.TeamB.Name}-{match.Id}"] = index++;
                }
            }

            foreach (var team in teams)
            {
                    nodeIndexes[$"team-{team.Name}"] = index++;
            }

            BuildGraph(matches, teams, t1, pointsTable ,maxPoints);
        }

        private void BuildGraph(List<Matches> matches, List<Team> teams, Team t1, List<PointsTable> pointsTable,int maxPoints)
        {
            int totalRemMatches = 0;
            foreach (var match in matches)
            {
                    int matchNodeIndex = nodeIndexes[$"match-{match.TeamA.Name}-{match.TeamB.Name}-{match.Id}"];
                    capacityMatrix[nodeIndexes["s"], matchNodeIndex] = 1;
                    totalRemMatches++;  
            }

            Console.WriteLine("Total remaining matches are : " + totalRemMatches);

            // From each match to its participating teams
            foreach (var match in matches)
            {
                    int matchNodeIndex = nodeIndexes[$"match-{match.TeamA.Name}-{match.TeamB.Name}-{match.Id}"];
                    int teamAIndex = nodeIndexes[$"team-{match.TeamA.Name}"];
                    int teamBIndex = nodeIndexes[$"team-{match.TeamB.Name}"];

                    capacityMatrix[matchNodeIndex, teamAIndex] = 1;
                    capacityMatrix[matchNodeIndex, teamBIndex] = 1;
            }

            // From each team to the sink, with capacity based on max points minus current points
            //maxPoints = teams.Count - 1;  // Assume max possible points is total teams - 1
            foreach (var team in teams)
            {
                if (team.TeamId != t1.TeamId)
                {
                    int teamIndex = nodeIndexes[$"team-{team.Name}"];
                    var pointsData = pointsTable.Find(pt => pt.TeamId == team.TeamId);

                    if (pointsData != null)
                    {
                        int remainingCapacity = maxPoints - pointsData.Points - 2;
                        capacityMatrix[teamIndex, nodeIndexes["t"]] = remainingCapacity;
                    }
                }
            }

            Console.WriteLine("Done .............,");
        }

        // Method to display the capacity matrix (adjacency matrix)
        public void DisplayCapacityMatrix()
        {
            Console.WriteLine("Capacity Matrix:");
            for (int i = 0; i < nodeCount; i++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    Console.Write($"{capacityMatrix[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public List<List<int>> GetGraph()
        {
            List<List<int>> graph = new List<List<int>>();

            // Convert the 2D array (capacityMatrix) into a List of Lists
            for (int i = 0; i < nodeCount; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < nodeCount; j++)
                {
                    row.Add(capacityMatrix[i, j]);
                }
                graph.Add(row);
            }

            return graph;
        }

        public int GetSinkIndex()
        {
            return nodeIndexes["t"];
        }

    }
}


//Capacity Matrix:
//0 1 1 1 1 1 1 1 1 1 1 1 1 0 0 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 1 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 1 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 1 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 1 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 1 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 1 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
//0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0