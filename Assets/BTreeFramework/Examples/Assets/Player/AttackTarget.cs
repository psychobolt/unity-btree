using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour {

    private EnemyAIActor target;

    private void Update()
    {
		if (target && Input.GetMouseButtonUp(0)) {
			target.TakeDamage(target.totalHP * 0.1f);
			Debug.Log("Attack!");
		}
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        target = collider.gameObject.GetComponent<EnemyAIActor>();
		if (target && target.IsBodyCollision()) {
			return;
		}
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<EnemyAIActor>() == target)
        {
            target = null;
        }
    }
}
