using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject target;
    public Slider slider;
	
	void Update () {
        EnemyActor enemyActor = target.GetComponent<EnemyActor>();
		if (enemyActor)
        {
            slider.value = enemyActor.health;
        }
	}
}
