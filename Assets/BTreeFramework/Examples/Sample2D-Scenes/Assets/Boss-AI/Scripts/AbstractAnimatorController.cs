using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AbstractAnimationController : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public abstract void ResetState();

    public abstract void LookLeft();

    public abstract void LookRight();

    public abstract void Idle();

    public abstract void Wake();

	public abstract void TakeDamage(HitType hitType, float damage);

    public abstract void Revive();

    public abstract void Move();

    public abstract void Halt();

    public abstract void PrepareMeleeAttack();

    public abstract void MeleeAttack();

    public abstract void WithdrawMeleeAttack();

    public abstract void PrepareRangeAttack();

    public abstract void RangeAttack();

    public abstract void WithdrawRangeAttack();
}
