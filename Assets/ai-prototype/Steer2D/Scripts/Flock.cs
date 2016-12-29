using System;
using UnityEngine;
using System.Collections.Generic;

namespace Steer2D
{
    public class Flock : SteeringBehaviour
    {
        public float NeighbourRadius = 1f;
        public float AlignmentWeight = .7f;
        public float CohesionWeigth = .5f;
        public float SeperationWeight = .2f;
        public bool DrawGizmos = false;

        List<SteeringAgent> neighbouringAgents = new List<SteeringAgent>();
        Vector2 currentPosition;

        public override Vector2 GetVelocity()
        {
            currentPosition = (Vector2)transform.position;

            UpdateNeighbouringAgents();

            return alignment() * AlignmentWeight + cohesion() * CohesionWeigth + seperation() * SeperationWeight;
        }

        Vector2 alignment()
        {
            Vector2 averageDirection = Vector2.zero;

            if (neighbouringAgents.Count == 0)
                return averageDirection;

            foreach (var agent in neighbouringAgents)
                averageDirection += agent.CurrentVelocity;

            averageDirection /= neighbouringAgents.Count;
            return averageDirection.normalized;
        }

        Vector2 cohesion()
        {
            Vector2 averagePosition = Vector2.zero;

            foreach (var agent in neighbouringAgents)
                averagePosition += (Vector2)agent.transform.position;

            averagePosition /= neighbouringAgents.Count;

            return (averagePosition - currentPosition).normalized;
        }

        Vector2 seperation()
        {
            Vector2 moveDirection = Vector2.zero;

            foreach (var agent in neighbouringAgents)
                moveDirection += (Vector2)agent.transform.position - currentPosition;

            return (moveDirection * -1);
        }

        void UpdateNeighbouringAgents()
        {
            neighbouringAgents.Clear();

            foreach (var agent in SteeringAgent.AgentList)
            {
                if (Vector3.Distance(agent.transform.position, currentPosition) < NeighbourRadius)
                    neighbouringAgents.Add(agent);
            }
        }

        void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, NeighbourRadius);
            }
        }
    }
}
