using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using Steer2D;

[RequireComponent(typeof (Rigidbody2D))]
public class EnemyAIActor : MonoBehaviour {

    public float health = 100;
    public float totalHP = 100;
    public float lowHealthPercentage = 0.5f;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    
    public bool IsLowHealth()
    {
        return (health / totalHP) < lowHealthPercentage;
    }

    public void Revive()
    {
        health = totalHP;
    }
}
