using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphNode : Node
{
    public enum Type
    {
        DEFAULT,
        SOURCE,
        DESTINATION,
        PATH,
        VISITED
    }

    Color[] colors =
    {
        Color.white,
        Color.green,
        Color.red,
        Color.yellow,
        Color.magenta
    };

    public struct Edge
    {
        public GraphNode nodeA;
        public GraphNode nodeB;
    }

    public GraphNode parent { get; set; } = null;
    public List<Edge> edges { get; set; } = new List<Edge>();
    public Type type { get; set; } = Type.DEFAULT;
    public bool visited { get; set; } = false;

    void Update()
    {
        // update node color using current node type
        GetComponent<Renderer>().material.color = colors[(int)type];
        // draw edges
        edges.ForEach(edge => Debug.DrawLine(edge.nodeA.transform.position, edge.nodeB.transform.position));
    }

    public static GraphNode GetNode(Type type)
    {
        // get first node of type
        var nodes = GetNodes<GraphNode>();
        var node = nodes.FirstOrDefault(node => node.type == type);

        return node;
    }

    public static void UnlinkNodes()
    {
        // clear all nodes edges
        var nodes = GetNodes<GraphNode>();
        nodes.ToList().ForEach(node => node.edges.Clear());
    }

    public static void LinkNodes(float radius)
    {
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
                // create edge from node to collider node
                Edge edge;
                edge.nodeA = node;
                edge.nodeB = colliderNode;

                // add edge to node edges
                node.edges.Add(edge);
            }
        }
    }
    
    public static void SetNodeType(Type typeFrom, Type typeTo)
    {
        var nodes = GetNodes<GraphNode>();

        var result = nodes.Where(node => node.type == typeFrom);
        result.ToList().ForEach(node => node.type = typeTo);
    }

    public static void ResetNodes()
    {
        // reset path and visited nodes
        SetNodeType(Type.PATH, Type.DEFAULT);
        SetNodeType(Type.VISITED, Type.DEFAULT);

        // set nodes not visited and parent to null
        var nodes = GetNodes<GraphNode>();
        nodes.ToList().ForEach(node => { node.visited = false; node.parent = null; });
    }
}
