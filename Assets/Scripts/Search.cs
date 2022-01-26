using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;

public static class Search
{
	public delegate bool SearchAlgorithm(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps);

	static public bool BuildPath(SearchAlgorithm searchAlgorithm, GraphNode source, GraphNode destination, ref List<GraphNode> path, int steps = int.MaxValue)
	{
		if (source == null || destination == null) return false;

		GraphNode.ResetNodes();

		bool found = searchAlgorithm(source, destination, ref path, steps);

		return found;
	}

	public static bool DFS(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
	{
		bool found = false;
		Stack<GraphNode> nodes = new Stack<GraphNode>();

		nodes.Push(source);

		int steps = 0;
		while (!found && nodes.Count > 0 && steps++ < maxSteps)
		{
			GraphNode node = nodes.Peek();
			node.visited = true;

			bool forward = false;

			foreach (var edge in node.edges)
			{
				if (!edge.visited)
				{
					nodes.Push(edge);
					forward = true;

					if (edge == destination)
					{
						found = true;
					}

					break;
				}
			}

			if (!forward)
			{
				nodes.Pop();
			}
		}

		path = new List<GraphNode>(nodes);
		path.Reverse();

		return found;
	}

	public static bool BFS(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
	{
		bool found = false;
		Queue<GraphNode> nodes = new Queue<GraphNode>();

		source.visited = true;
		nodes.Enqueue(source);
		int steps = 0;

		while (!found && nodes.Count > 0 && steps++ < maxSteps)
		{
			var node = nodes.Dequeue();

			foreach (var edge in node.edges)
			{
				if (!edge.visited)
				{
					edge.visited = true;
					edge.parent = node;
					nodes.Enqueue(edge);
				}

				if (edge == destination)
				{
					found = true;
					break;
				}
			}
		}

		if (found)
		{
			path = new List<GraphNode>();
			CreatePathFromParents(destination, ref path);
		}
		else
		{
			path = nodes.ToList();
		}

		return found;
	}

	public static bool Dijkstra(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
	{
		bool found = false;
		var nodes = new SimplePriorityQueue<GraphNode>();

		source.cost = 0;
		nodes.Enqueue(source, source.cost);

		int steps = 0;
		while (!found && nodes.Count > 0 && steps++ < maxSteps)
		{
			var node = nodes.Dequeue();

			if (node == destination)
			{
				found = true;
				break;
			}
			foreach (var neighbor in node.edges)
			{
				neighbor.visited = true;

				float cost = node.cost + Vector3.Distance(neighbor.transform.position, node.transform.position);
				if (cost < neighbor.cost)
				{
					neighbor.cost = cost;
					neighbor.parent = node;
					nodes.EnqueueWithoutDuplicates(neighbor, neighbor.cost);
				}
			}
		}
		if (found)
		{
			path = new List<GraphNode>();
			CreatePathFromParents(destination, ref path);
		}
		else
		{
			path = nodes.ToList();
		}

		return found;
	}

	public static bool AStar(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
	{
		bool found = false;
		var nodes = new SimplePriorityQueue<GraphNode>();

		source.cost = 0;
		float heuristic = Vector3.Distance(source.transform.position, destination.transform.position);
		nodes.Enqueue(source, source.cost + heuristic);

		int steps = 0;
		while (!found && nodes.Count > 0 && steps++ < maxSteps)
		{
			var node = nodes.Dequeue();

			if (node == destination)
			{
				found = true;
				break;
			}
			foreach (var neighbor in node.edges)
			{
				neighbor.visited = true;
				float cost = node.cost + Vector3.Distance(neighbor.transform.position, node.transform.position);

				if (cost < neighbor.cost)
				{
					neighbor.cost = cost;
					neighbor.parent = node;
					heuristic = Vector3.Distance(neighbor.transform.position, destination.transform.position);
					nodes.EnqueueWithoutDuplicates(neighbor, neighbor.cost + heuristic);
				}
			}
		}
		if (found)
		{
			path = new List<GraphNode>();
			CreatePathFromParents(destination, ref path);
		}
		else
		{
			path = nodes.ToList();
		}

		return found;
	}

	public static void CreatePathFromParents(GraphNode destination, ref List<GraphNode> path)
	{
		var node = destination;

		while (node != null)
		{
			path.Add(node);
			node = node.parent;
		}

		path.Reverse();
	}
}