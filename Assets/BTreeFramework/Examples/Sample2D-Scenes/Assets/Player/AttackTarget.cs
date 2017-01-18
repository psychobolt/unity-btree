using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour {

    private EnemyActor target;

    private void Update()
    {
		if (target && Input.GetMouseButtonUp(0)) {
			target.TakeDamage(target.totalHP * 0.1f);
			Debug.Log("Attack!");
		}
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyActor target = collider.gameObject.GetComponent<EnemyActor>();
        if (!target) {
            target = collider.gameObject.GetComponentInParent<EnemyActor>();
        }
		if (target && target.IsBodyCollision()) {
            this.target = target;
		}
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<EnemyActor>() == target ||
            collider.gameObject.GetComponentInParent<EnemyActor>() == target)
        {
            target = null;
        }
    }
}
