using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationController
{
    private Animator animator;

    public AnimationController(Animator animator)
    {
        this.animator = animator;
    }

    public void Reset()
    {
        Halt();
        WithdrawMeleeAttack();
        WithdrawRangeAttack();
        Idle();
    }

    public void LookLeft()
    {
        animator.SetFloat("lookX", -1);
    }

    public void LookRight()
    {
        animator.SetFloat("lookX", 1);
    }

    public void Idle()
    {
        animator.SetBool("idle", true);
    }

    public void Wake()
    {
        animator.SetBool("idle", false);
    }

    public void Disappear()
    {
        Reset();
        animator.SetBool("idle", false);
        animator.SetBool("disappear", true);
    }

    public void Appear()
    {
        animator.SetBool("disappear", false);
    }

    public void Move()
    {
        animator.SetBool("move", true);
    }

    public void Halt()
    {
        animator.SetBool("move", false);
    }

    public void PrepareMeleeAttack()
    {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", true);
    }

    public void MeleeAttack()
    {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", false);
    }

    public void WithdrawMeleeAttack()
    {
        animator.SetBool("prepare", false);
        animator.SetBool("melee", false);
    }

    public void PrepareRangeAttack()
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", true);
    }

    public void RangeAttack()
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
    }

    public void WithdrawRangeAttack()
    {
        animator.SetBool("range", false);
        animator.SetBool("prepare", false);
    }
}
