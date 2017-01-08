using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour {

	public bool isBodyCollision;

	private EnemyAIActor actor;

	void Start() {
		actor = transform.root.GetComponent<EnemyAIActor>();
		if (actor == null) {
			actor = GetComponent<EnemyAIActor>();
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			actor.SetTarget(collider.gameObject, isBodyCollision);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			actor.SetTarget(null, false);
		}
	}
}
