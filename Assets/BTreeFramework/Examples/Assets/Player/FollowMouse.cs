using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    public float offsetX;
    public float offsetY;
	
	// Update is called once per frame
	void Update () {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.transform.position
            = new Vector3(offsetX + worldPos.x, offsetY + worldPos.y, gameObject.transform.position.z);
	}
}
