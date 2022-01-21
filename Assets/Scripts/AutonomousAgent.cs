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

        void Update()
        {
                //SEEK / FLEE
                GameObject[] gameObjects = perception.GetGameObjects();

                if (gameObjects.Length == 0)
                {
                        movement.ApplyForce(steering.Wander(this));
                }
                else
                {
                        foreach (GameObject go in gameObjects)
                        {
                                movement.ApplyForce(steering.Seek(this, go) * agentData.seekWeight);
                                movement.ApplyForce(steering.Flee(this, go) * agentData.fleeWeight);
                        }
                }

                //FLOCKING
                gameObjects = flockPerception.GetGameObjects();
                if (gameObjects.Length > 0)
                {
                        movement.ApplyForce(steering.Cohesion(this, gameObjects) * agentData.cohesionWeight);
                        movement.ApplyForce(steering.Separation(this, gameObjects, agentData.separationRadius) * agentData.separationWeight);
                        movement.ApplyForce(steering.Alignment(this, gameObjects) * agentData.alignmentWeight);
                }

                // obstacle avoidance
                if (obstaclePerception.IsObstacleInFront())
                {
                        Vector3 direction = obstaclePerception.GetOpenDirection();
                        movement.ApplyForce(steering.CalculateSteering(this, direction) * agentData.obstacleWeight);
                }

                //WANDER
                if (movement.acceleration.sqrMagnitude > movement.maxForce * 0.1f)
                {
                        movement.ApplyForce(steering.Wander(this));
                }
        }
}