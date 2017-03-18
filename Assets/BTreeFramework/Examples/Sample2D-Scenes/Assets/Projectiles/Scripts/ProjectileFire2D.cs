using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample2D
{
    public class ProjectileFire2D : AbstractProjectileBehaviour
    {
        public GameObject prefab;
        public GameObject target;
        public GameObject fireFrom;
        public float projectileVelocity;
        public float fireDelay;
        public float fireRate;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Decrement timer until its time to fire
            if (fireDelay > 1.0)
            {
                fireDelay -= Time.deltaTime;
            }
            //Fire
            else
            {
                Fire();
                //Reset the fire timer
                fireDelay += fireRate;
            }
        }

        public override void Fire()
        {
            //Get the quaternion rotation from the shooter to the target
            Vector2 destination = target.transform.position;
            Vector2 center = fireFrom.transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

            //Instantiate the projectile with the correct angle and position
            GameObject projectile = Instantiate(prefab, center, rot) as GameObject;


            //Get the rigid body 2D and apply a force towards the target with given velocity
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
            rigidbody.velocity = (destination - center).normalized * projectileVelocity;
        }
    }
}
