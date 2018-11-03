using System.Collections.Generic;

namespace SneakingOut
{
    //Data Structure for a graph node
    public class Node
    {
        public int Index { get; } //Essentially the name or identifier of the node

        public List<Node> Neighbors { get; } 
        public List<Edge> Edges { get; }
        
        public bool IsVisited { get; set; } = false;

        public int Distance { get; set; } = int.MaxValue;  //Initially set to infinity

        public Node(int index)
        {
            Index = index;
            Neighbors = new List<Node>();
            Edges = new List<Edge>();
        }
    }

    //Data structure for a graph edge
    public class Edge
    {
        //Each edge has a start and end node, as well as a weight (i.e. how long it takes to navigate)
        public Node Start { get; }
        public Node End { get; }
        public int Weight { get; }

        public Edge(Node start, Node end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }
    }

    //Graph contains all the nodes and edges in one data structure.
    public class Graph
    {
        public List<Node> Nodes { get; }
        public List<Edge> Edges { get; }

        public Graph(List<Node> nodes, List<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}