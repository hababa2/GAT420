using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNodeSelector : MonoBehaviour
{
    public GameObject selector;
    public LayerMask layerMask;

    public bool IsActive { get { return selector.activeSelf; } }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
        {
            // get node component on collider
            var node = hitInfo.collider.GetComponent<GraphNode>();
            if (node == null) return;

            // set selection object active at node position
            selector.SetActive(true);
            selector.transform.position = node.transform.position;

            if (Input.GetMouseButtonDown(1))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    // clear current source node and set new source node
                    GraphNode.SetNodeType(GraphNode.Type.SOURCE, GraphNode.Type.DEFAULT);
                    node.type = GraphNode.Type.SOURCE;
                    GraphNode.ResetNodes();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    // clear current destination node and set new destination node
                    GraphNode.SetNodeType(GraphNode.Type.DESTINATION, GraphNode.Type.DEFAULT);
                    node.type = GraphNode.Type.DESTINATION;
                    GraphNode.ResetNodes();
                }
            }
        }
        else
        {
            selector.SetActive(false);
        }
    }
}
