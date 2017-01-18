using UnityEngine;
using System.Collections.Generic;

namespace Steer2D
{
    public class Arrive : SteeringBehaviour 
    {
        public Vector2 TargetPoint = Vector2.zero;
        public float SlowRadius = 1;
        public float StopRadius = 0.2f;
        public bool DrawGizmos = false;

        public override Vector2 GetVelocity()
        {
            float distance = Vector3.Distance(transform.position, (Vector3)TargetPoint);
            Vector2 desiredVelocity = (TargetPoint - (Vector2)transform.position).normalized;

            if (distance < StopRadius)
                desiredVelocity = Vector2.zero;
            else if (distance < SlowRadius)
                desiredVelocity = desiredVelocity * agent.MaxVelocity * ((distance - StopRadius) / (SlowRadius - StopRadius));
            else
                desiredVelocity = desiredVelocity * agent.MaxVelocity;

            return desiredVelocity - agent.CurrentVelocity;
        }

        void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere((Vector3)TargetPoint, SlowRadius);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere((Vector3)TargetPoint, StopRadius);
            }
        }
    }
}