using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphNode : Node
{
	public GraphNode parent { get; set; } = null;
	public bool visited { get; set; } = false;
	public float cost { get; set; } = float.MaxValue;
	public List<GraphNode> edges { get; set; } = new List<GraphNode>();

	public static void UnlinkNodes()
	{
		// clear all nodes edges
		var nodes = GetNodes<GraphNode>();
		nodes.ToList().ForEach(node => node.edges.Clear());
	}

	public static void LinkNodes(float radius)
	{
		// link all nodes to neighbor nodes within radius
		var nodes = GetNodes<GraphNode>();
		nodes.ToList().ForEach(node => LinkNeighbors(node, radius));
	}

	public static void LinkNeighbors(GraphNode node, float radius)
	{
		// find nodes in sphere radius
		Collider[] colliders = Physics.OverlapSphere(node.transform.position, radius);
		foreach (Collider collider in colliders)
		{
			// get node in collider
			GraphNode colliderNode = collider.GetComponent<GraphNode>();
			if (colliderNode != null && colliderNode != node)
			{
				// add edge to node edges
				node.edges.Add(colliderNode);
			}
		}
	}

	public static void ResetNodes()
	{
		// reset nodes visited and parent
		var nodes = GetNodes<GraphNode>();
		nodes.ToList().ForEach(node => { node.visited = false; node.parent = null; node.cost = float.MaxValue; });
	}
}
