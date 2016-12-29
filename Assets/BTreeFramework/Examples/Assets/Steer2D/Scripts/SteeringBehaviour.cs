using UnityEngine;
using System.Collections;

namespace Steer2D
{
	[RequireComponent(typeof(SteeringAgent))]
	public abstract class SteeringBehaviour : MonoBehaviour {

        public float Weight = 1;

        protected SteeringAgent agent;

        public abstract Vector2 GetVelocity();

		void Start () {
            agent = GetComponent<SteeringAgent>();
            agent.RegisterSteeringBehaviour(this);
		}

		void OnDestroy()
		{
            if (agent != null)
                agent.DeregisterSteeringBehaviour(this);
		}   
	}
}