using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphNodeCreator : MonoBehaviour
{
	public GameObject nodePrefab;
	public LayerMask layerMask;
	public float neighborRadius = 3;
	public float grid = 2;
	public GraphNodeSelector selector;

	void Update()
	{
		if ((Input.GetMouseButtonDown(1) || 
			(Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))) && 
			selector.IsActive == false)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, 100))
			{
				// check if hit layer mask
				if (((1 << hitInfo.collider.gameObject.layer) & layerMask) == 0) return;

				// set position on grid
				Vector3 position = hitInfo.point;
				if (grid != 0)
				{
					position.x = Mathf.FloorToInt(position.x / grid) * grid;
					position.z = Mathf.FloorToInt(position.z / grid) * grid;
					
					// make sure position doesn't have a node already
					if (Physics.CheckSphere(position, grid * 0.5f, 1 << nodePrefab.layer))
					{
						return;
					}
				}

				// create node
				Instantiate(nodePrefab, position, Quaternion.identity, transform);

				// unlink/link nodes within radius
				GraphNode.UnlinkNodes();
				GraphNode.LinkNodes(neighborRadius);
			}
		}
	}

	public void ClearNodes()
	{
		var nodes = Node.GetNodes<GraphNode>();
		nodes.ToList().ForEach(node => Destroy(node.gameObject));

		//foreach (var node in nodes)
		//{
		//	Destroy(node.gameObject);
		//}
	}
}
