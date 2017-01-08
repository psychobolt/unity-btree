using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DeathAnimationController : AbstractAnimationController
{
    public override void ResetState()
    {
        Halt();
        WithdrawMeleeAttack();
        WithdrawRangeAttack();
        Idle();
    }

    public override void LookLeft()
    {
        animator.SetFloat("lookX", -1);
    }

    public override void LookRight()
    {
        animator.SetFloat("lookX", 1);
    }

    public override void Idle()
    {
        animator.SetBool("idle", true);
    }

    public override void Wake()
    {
        animator.SetBool("idle", false);
    }

    public override void Disappear()
    {
        ResetState();
        animator.SetBool("idle", false);
        animator.SetBool("disappear", true);
        animator.SetBool("appear", false);
    }

    public override void Appear()
    {
        animator.SetBool("disappear", false);
        animator.SetBool("appear", true);
    }

    public override void Move()
    {
        WithdrawMeleeAttack();
        WithdrawRangeAttack();
        animator.SetBool("move", true);
    }

    public override void Halt()
    {
        animator.SetBool("move", false);
    }

    public override void PrepareMeleeAttack()
    {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", true);
    }

    public override void MeleeAttack()
    {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", false);
    }

    public override void WithdrawMeleeAttack()
    {
        animator.SetBool("prepare", false);
        animator.SetBool("melee", false);
    }

    public override void PrepareRangeAttack()
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", true);
    }

    public override void RangeAttack()
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
    }

    public override void WithdrawRangeAttack()
    {
        animator.SetBool("range", false);
        animator.SetBool("prepare", false);
    }
}
