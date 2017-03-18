using UnityEngine;

public class CollisionController : MonoBehaviour {

	public bool isBodyCollision;

	private EnemyActor actor;

	void Start() {
		actor = transform.root.GetComponentInChildren<EnemyActor>();
		if (actor == null) {
			actor = GetComponent<EnemyActor>();
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
