using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fano.Logging.Core;

namespace SneakingOut
{
    public class SneakOut
    {
        private readonly ILogger _log;
        

        public SneakOut(ILogger log)
        {
            _log = log;
        }
        
        public void Run(List<string> input)
        {
            //Parse the input into a list of graphs and calculate the shortest distance for each one
            var graphs = new List<Graph>();
            
            //Read the first line of the file as an integer and remove it
            int testCaseCount = int.Parse(input[0]);
            input.RemoveAt(0);

            //First line contained number of test cases (graphs), run this code for each test case.
            for (int i = 0; i < testCaseCount; i++)
            {
                //Read the number of nodes and edges in the graph
                var graphData = input[0].Split(' ');
                int nodeCount = int.Parse(graphData[0]);
                int edgeCount = int.Parse(graphData[1]);

                //Remove the line from the input
                input.RemoveAt(0);
                
                //Build the nodes and the edges
                var nodes = BuildNodes(nodeCount);
                var edges = BuildEdges(input, edgeCount, nodes);

                //remove all the lines we just parsed
                input.RemoveRange(0, edgeCount);

                //Create the graph with the nodes and edges and add it to the list
                graphs.Add(new Graph(nodes, edges));
            }

            //For each graph, calculate the shortest distance and print it to the console
            for (var i = 0; i < graphs.Count; i++)
            {
                var testCase = graphs[i];
                int dist = CalculateShortestDistance(testCase);
                _log.Info($"Test Case {i} - Shortest Distance: {dist}");
            }
        }

        private List<Node> BuildNodes(int nodeCount)
        {
            //Create a list of nodes with indexes going from 1 -> nodeCount
            var nodes = new List<Node>();
            for (int i = 1; i <= nodeCount; i++)
            {
                nodes.Add(new Node(i));
            }
            return nodes;
        }

        private List<Edge> BuildEdges(IReadOnlyList<string> input, int edgeCount, IReadOnlyList<Node> nodes)
        {
            //A little complicated..for each edge definition, find the start and end node from the node list to insert them into the edge class.
            var edges = new List<Edge>();
            for (int i = 0; i < edgeCount; i++)
            {
                //Read the edge values
                var edgeData = input[i].Split(' ');
                int startIndex = int.Parse(edgeData[0]);
                int endIndex = int.Parse(edgeData[1]);
                int cost = int.Parse(edgeData[2]);

                //Find the start and end nodes
                var start = nodes.Single(n => n.Index == startIndex);
                var end = nodes.Single(n => n.Index == endIndex);

                //associate the start and end nodes by adding them as neighbors
                start.Neighbors.Add(end);
                end.Neighbors.Add(start);

                //Since this is not a directed graph, add an edge from start -> end && from end -> start
                var toEdge = new Edge(start, end, cost);
                start.Edges.Add(toEdge);
                
                var fromEdge = new Edge(end, start, cost);
                end.Edges.Add(fromEdge);

                //Add both edges to the list of all edges, for convenience
                edges.Add(toEdge);
                edges.Add(fromEdge);
            }

            return edges;
        }

        private int CalculateShortestDistance(Graph graph)
        {
            //This is Dijkstra’s shortest path algorithm, it will navigate through each node in the graph
            //starting with our starting node (index 1) and it will set the distance value of each neighbor node 
            //to the shortest distance from it to the starting node.

            //start with an empty list of already visited nodes
            var visited = new List<Node>();

            //from the graph, get the starting node and set it's distance to 0
            //this node is the closest to the starting node because it IS the starting node.
            var start = graph.Nodes.Single(n => n.Index == 1);
            start.Distance = 0;
            
            //Loop until we have visited every node in the graph
            while (visited.Count != graph.Nodes.Count)
            {
                //Grab the next node in the graph that hasn't been visited with the shortest distance
                // --In the first iteration, it will grab our starting node since its distance is 0
                var min = graph.Nodes.Where(n => !n.IsVisited).OrderBy(n => n.Distance).First();

                //Mark it as visited
                visited.Add(min);
                min.IsVisited = true;

                //Loop through this node's neighbors and calculate the distance to each one
                foreach (var neighbor in min.Neighbors)
                {
                    //grab the edge that connects this neighbor
                    var edge = min.Edges.Single(e => e.End.Index == neighbor.Index);
                    
                    //if the sum of this node's distance and the edge is lower than the distance currently set on neighbor, update it.
                    neighbor.Distance = Math.Min(min.Distance + edge.Weight, neighbor.Distance);
                }
            }

            //Once we have visited all the nodes, every node in the graph has a variable Distance that indicates how close it is to the starting node.
            //Simply grab the ending node, and give back its distance.
            var end = visited.OrderByDescending(n => n.Index).First();
            return end.Distance;
        }
    }
}