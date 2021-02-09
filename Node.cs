using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_6
{
	 class Node
	{
		public int Value { get; set; }
		public Node(int number)
		{
			Value = number;
		}
        
    }
     class Graph
	{
		public List<Node> Vertexes = new List<Node>();
		public List<Edge> Edges = new List<Edge>();
		public int VertexesCount => Vertexes.Count;
		public int EdgeCount => Edges.Count;

		public void AddVertex(Node vertex)
		{
			Vertexes.Add(vertex);
		}
		public void AddEdge(Node from, Node to, int x)
		{
			var edge = new Edge(from, to, x);
			Edges.Add(edge);
		}
		
	}
	
	class Edge
	{
		public int Weight { get; set; }
		public Node To { get; set; }
		public Node From { get; set; }

		public Edge(Node from, Node to, int weight)
		{
			From = from;
			To = to;
			Weight = weight;
		}
		//public override string ToString()
		//{
		///	return string.Format(this.From + " " + this.To + " " + this.Weight);
		//}
	}
}