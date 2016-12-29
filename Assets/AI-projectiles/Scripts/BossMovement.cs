using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement: MonoBehaviour {

    public float speed = 1.5f;
    public GameObject character;
    private Vector3 target;
    public Animator animator;
    private SpriteRenderer SpriteR;


	// Use this for initialization
	void Start () {
        target = transform.position;
        SpriteR = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        target = character.transform.position;
        //target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, target, 
                            speed * Time.deltaTime);

        if((transform.position.x - character.transform.position.x) > 0 )
        {
            SpriteR.flipX = true;
        }
        else
        {
            SpriteR.flipX = false;
        }
        //Animator.SetFloat("Speed", );

    }
}
