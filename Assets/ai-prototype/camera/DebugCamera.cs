using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {

    public float speed = 10.0f;
    public KeyCode keyRight = KeyCode.RightArrow;
    public KeyCode keyLeft = KeyCode.LeftArrow;
    public KeyCode keyUp = KeyCode.UpArrow;
    public KeyCode keyDown = KeyCode.DownArrow;

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(keyRight))
        {
            transform.Translate(new Vector3(Time.deltaTime * speed, 0.0f, 0.0f));
        }
        if (Input.GetKey(keyLeft))
        {
            transform.Translate(new Vector3(Time.deltaTime * -speed, 0.0f, 0.0f));
        }
        if (Input.GetKey(keyUp))
        {
            transform.Translate(new Vector3(0.0f, Time.deltaTime * speed, 0.0f));
        }
        if (Input.GetKey(keyDown))
        {
            transform.Translate(new Vector3(0.0f, Time.deltaTime * -speed, 0.0f));
        }
    }
}
