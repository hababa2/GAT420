using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{
	[SerializeField] private float radius;
	[SerializeField] private float maxAngle;

	public override GameObject[] GetGameObjects()
	{
		List<GameObject> result = new List<GameObject>();

		Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

		foreach(Collider c in colliders)
		{
			if(c.gameObject != gameObject && (tagName == "" || c.CompareTag(tagName)))
			{
				Vector3 direction = (c.transform.position - transform.position).normalized;
				float cos = Vector3.Dot(transform.forward, direction);
				float angle = Mathf.Acos(cos) * Mathf.Rad2Deg;

				if (angle <= maxAngle)
				{
					result.Add(c.gameObject);
				}
			}
		}

		return result.ToArray();
	}
}
