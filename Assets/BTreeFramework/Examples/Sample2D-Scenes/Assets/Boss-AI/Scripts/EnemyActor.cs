using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using Steer2D;

[RequireComponent(typeof (Rigidbody2D))]
public class EnemyActor : MonoBehaviour {

    public float health = 100;
    public float totalHP = 100;

	private GameObject target;
	private bool bodyCollision;

	void Start() {
		tag = "Enemy";
	}

    public void TakeDamage(float damage)
    {
		float health = this.health - damage;
		this.health = health < 0 ? 0 : health;
    }

	public void SetTarget(GameObject target) {
		this.target = target;
	}

	public void SetTarget(GameObject target, bool bodyCollision) {
		SetTarget(target);
		this.bodyCollision = bodyCollision;
	}

	public GameObject GetTarget() {
		return target;
	}

	public bool IsBodyCollision() {
		return bodyCollision;
	}
}
