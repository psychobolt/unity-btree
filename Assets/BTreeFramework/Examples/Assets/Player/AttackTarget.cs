using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour {

    private EnemyAIActor target;

    private void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        target = collider.gameObject.GetComponent<EnemyAIActor>();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<EnemyAIActor>() == target)
        {
            target = null;
        }
    }
}
