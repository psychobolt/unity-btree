using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayWithTarget : MonoBehaviour {

    public GameObject target;

    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

	void Update () {
        transform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, target.transform.position.z + offset.z);
	}
}
