using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
	[SerializeField] [Range(0, 45)] private float wanderDisplacement;
	[SerializeField] [Range(0, 5)] private float wanderRadius;
	[SerializeField] [Range(0, 5)] private float wanderDistance;

	private float wanderAngle = 0;

	public Vector3 Wander(AutonomousAgent agent)
	{
		wanderAngle += Random.Range(-wanderDisplacement, wanderDisplacement);

		Quaternion rotation = Quaternion.AngleAxis(wanderAngle, Vector3.up);
		Vector3 point = rotation * (Vector3.forward * wanderRadius);
		Vector3 forward = agent.transform.forward * wanderDistance;

		return CalculateSteering(agent, forward + point);
	}

	public Vector3 Seek(AutonomousAgent agent, GameObject target)
	{
		return CalculateSteering(agent, target.transform.position - agent.transform.position);
	}

	public Vector3 Flee(AutonomousAgent agent, GameObject target)
	{
		return CalculateSteering(agent, agent.transform.position - target.transform.position);
	}

	public Vector3 Cohesion(AutonomousAgent agent, GameObject[] targets)
	{
		Vector3 center = Vector3.zero;

		foreach(GameObject t in targets)
		{
			center += t.transform.position;
		}

		center /= targets.Length;

		return CalculateSteering(agent, center - agent.transform.position);
	}

	public Vector3 Separation(AutonomousAgent agent, GameObject[] targets, float radius)
	{
		Vector3 separation = Vector3.zero;

		foreach (GameObject target in targets)
		{
			Vector3 direction = (agent.transform.position - target.transform.position);
			if (direction.magnitude < radius)
			{
				separation += direction / direction.sqrMagnitude;
			}
		}

		return CalculateSteering(agent, separation);
	}

	public Vector3 Alignment(AutonomousAgent agent, GameObject[] targets)
	{
		Vector3 averageVelocity = Vector3.zero;

		foreach (GameObject target in targets)
		{
			averageVelocity += target.GetComponent<AutonomousAgent>().velocity;
		}

		averageVelocity /= targets.Length;

		return CalculateSteering(agent, averageVelocity);
	}

	public Vector3 CalculateSteering(AutonomousAgent agent, Vector3 vector)
	{
		Vector3 dir = vector.normalized;
		Vector3 desired = dir * agent.maxSpeed;
		Vector3 steer = desired - agent.velocity;

		return Vector3.ClampMagnitude(steer, agent.maxForce);
	}
}
