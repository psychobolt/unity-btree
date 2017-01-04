using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using Steer2D;

[RequireComponent(typeof (Rigidbody2D))]
public class EnemyAIActor : MonoBehaviour {

    public float health = 100;
    public float totalHP = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Revive()
    {
        health = totalHP;
    }
}
