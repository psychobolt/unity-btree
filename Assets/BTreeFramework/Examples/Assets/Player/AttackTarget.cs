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

    void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyAIActor target = collider.gameObject.GetComponent<EnemyAIActor>();
        if (!target) {
            target = collider.gameObject.GetComponentInParent<EnemyAIActor>();
        }
		if (target && target.IsBodyCollision()) {
            this.target = target;
		}
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<EnemyAIActor>() == target ||
            collider.gameObject.GetComponentInParent<EnemyAIActor>() == target)
        {
            target = null;
        }
    }
}
