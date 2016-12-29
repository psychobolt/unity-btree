using System;
using UnityEngine;

namespace Steer2D
{
    public class Flee : SteeringBehaviour
    {
        public Vector2 TargetPoint = Vector2.zero;
        public float FleeRadius = 1;
        public bool DrawGizmos = false;

        public override Vector2 GetVelocity()
        {
            float distance = Vector3.Distance(transform.position, TargetPoint);

            if (distance < FleeRadius)
                return -(((TargetPoint - (Vector2)transform.position).normalized * agent.MaxVelocity) - agent.CurrentVelocity);
            else
                return Vector2.zero;
        }

        void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(TargetPoint, FleeRadius);
            }
        }
    }
}
