using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AbstractAnimationController : MonoBehaviour
{
    protected Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public abstract void ResetState();

    public abstract void LookLeft();

    public abstract void LookRight();

    public abstract void Idle();

    public abstract void Wake();

    public abstract void Disappear();

    public abstract void Appear();

    public abstract void Move();

    public abstract void Halt();

    public abstract void PrepareMeleeAttack();

    public abstract void MeleeAttack();

    public abstract void WithdrawMeleeAttack();

    public abstract void PrepareRangeAttack();

    public abstract void RangeAttack();

    public abstract void WithdrawRangeAttack();
}
