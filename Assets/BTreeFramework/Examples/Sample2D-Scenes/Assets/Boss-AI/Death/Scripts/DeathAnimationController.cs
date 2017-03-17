using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DeathAnimationController : AbstractAnimationController
{
	SpriteRenderer spriteRenderer;
	GameObject takeDamage;

	protected override void Start ()
	{
		base.Start ();
		spriteRenderer = GetComponent<SpriteRenderer>();
		takeDamage = transform.FindChild("TakeDamage").gameObject;
	}

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

	public override void TakeDamage(HitType hitType, float damage)
    {
		ResetState();
		animator.SetBool("idle", false);
        if (hitType == HitType.POINTS)
        {
            spriteRenderer.enabled = false;
            takeDamage.SetActive(true);
        }
        else
        {
            Disappear();
        }
    }

    private void Disappear()
    {
        takeDamage.SetActive(false);
        spriteRenderer.enabled = true;
        animator.SetBool("disappear", true);
        animator.SetBool("appear", false);
    }

    public override void Revive()
    {
        takeDamage.SetActive(false);
        spriteRenderer.enabled = true;
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
