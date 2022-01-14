using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousAgent : Agent
{
        [SerializeField] private Perception perception;
        [SerializeField] private Perception flockPerception;
        [SerializeField] private ObstaclePerception obstaclePerception;
        [SerializeField] private Steering steering;
        [SerializeField] private AutonomousAgentData agentData;

        public float maxSpeed { get { return agentData.maxSpeed; } }
        public float maxForce { get { return agentData.maxForce; } }

        public Vector3 velocity { get; set; } = Vector3.zero;

        void Update()
        {
                Vector3 acceleration = Vector3.zero;

                //SEEK / FLEE
                GameObject[] gameObjects = perception.GetGameObjects();

                if (gameObjects.Length == 0)
                {
                        acceleration += steering.Wander(this);
                }
                else
                {
                        foreach (GameObject go in gameObjects)
                        {
                                //Debug.DrawLine(transform.position, go.transform.position);

                                acceleration += steering.Seek(this, go) * agentData.seekWeight;
                                acceleration += steering.Flee(this, go) * agentData.fleeWeight;
                        }
                }

                //FLOCKING
                gameObjects = flockPerception.GetGameObjects();
                if (gameObjects.Length > 0)
                {
                        acceleration += steering.Cohesion(this, gameObjects) * agentData.cohesionWeight;
                        acceleration += steering.Separation(this, gameObjects, agentData.separationRadius) * agentData.separationWeight;
                        acceleration += steering.Alignment(this, gameObjects) * agentData.alignmentWeight;
                }

                // obstacle avoidance
                if (obstaclePerception.IsObstacleInFront())
                {
                        Vector3 direction = obstaclePerception.GetOpenDirection();
                        acceleration += steering.CalculateSteering(this, direction) * agentData.obstacleWeight;
                }

                //WANDER
                if (velocity.sqrMagnitude > 0.1f)
                {
                        transform.rotation = Quaternion.LookRotation(velocity.normalized);
                }

                velocity += acceleration * Time.deltaTime;
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
                transform.position += velocity * Time.deltaTime;

                transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20));
        }
}