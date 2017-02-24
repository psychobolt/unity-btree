using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithTarget : MonoBehaviour {

    public GameObject target;

    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;

    private Vector3 initialScale;
    
    void Start () {
        initialScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
	
	void Update () {
        float scaleX = freezeX ? 1 : target.transform.localScale.x;
        float scaleY = freezeY ? 1 : target.transform.localScale.y;
        float scaleZ = freezeZ ? 1 : target.transform.localScale.z;
        transform.localScale = new Vector3(scaleX * target.transform.localScale.x, scaleY * target.transform.localScale.y, scaleZ * target.transform.localScale.z);
	}
}
