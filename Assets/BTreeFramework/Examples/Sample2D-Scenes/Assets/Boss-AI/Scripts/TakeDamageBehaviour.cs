using BTree;
using UnityEngine;

[RequireComponent(typeof(EnemyActor))]
[AddComponentMenu("AI Behaviour Tree/Take Damage")]
public class TakeDamageBehaviour : AbstractBTreeBehaviour
{
    public float damageTime = 1.0f;
    public float reviveTime = 1.0f;
    public float cooldownTime = 0.0f;

	public HitType hitType;
    public int hitValue;
    public int revivePrecentage = 100;

    private EnemyActor actor;
	private bool cooldown = false;
	private float timeElapsed;
	private float lastHealth;
	private float deltaHP;

    protected override void Start()
    {
        base.Start();
        actor = gameObject.GetComponent<EnemyActor>();
		lastHealth = actor.health;
    }

	void Update()
	{
		if (cooldown) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed > cooldownTime) {
				EndCooldown();
			}
		}
	}
    
    public bool IsHitValue()
    {
		deltaHP = lastHealth - actor.health;
		bool isHitValue = false;
		switch (hitType) 
		{
			case HitType.POINTS:
				isHitValue = deltaHP == hitValue;
				break;
			case HitType.DMG_PERCENTAGE:
				isHitValue = (deltaHP / lastHealth * 100) >= hitValue;
				break;
			case HitType.HP_PRECENTAGE:
				isHitValue = ((actor.totalHP - actor.health) / actor.totalHP * 100) >= hitValue;
				break;
		}
		lastHealth = actor.health;
		return isHitValue;
    }

	public BehaviourTree.State Cooldown(BehaviourTreeNode<System.Object> node)
	{
		if (cooldown) {
            return BehaviourTree.State.FAILURE;
		}
		return BehaviourTree.State.SUCCESS;
	}

    public BehaviourTree.State AnimateTakeDamage(BehaviourTreeNode<float> node)
    {
        if (damageTime <= 0)
        {
            return BehaviourTree.State.SUCCESS;
        }
        if (node.Result == 0)
        {
			animationController.TakeDamage(hitType, deltaHP);
        }
        node.Result += Time.deltaTime;
        if (node.Result > damageTime)
        {
			node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State AnimateRevive(BehaviourTreeNode<float> node)
    {
        if (reviveTime <= 0)
        {
            return BehaviourTree.State.SUCCESS;
        }
        if (node.Result == 0)
        {
            animationController.Revive();
        }
        node.Result += Time.deltaTime;
        if (node.Result > reviveTime)
        {
			node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    private void Revive()
    {
        if (revivePrecentage > 0)
        {
            actor.health += (actor.totalHP - actor.health) * (revivePrecentage / 100);
            lastHealth = actor.health;
        }
    }

	private void StartCooldown() {
		cooldown = true;
		timeElapsed = 0;
	}

	private void EndCooldown() {
		cooldown = false;
		timeElapsed = 0;
	}

    protected override BehaviourTree.Node Initialize()
    {
        return new BinaryTreeNode(
			() => IsHitValue(),
			new SequenceTreeNode(new BehaviourTree.Node[]
			{
				new ActionTreeNode<System.Object>(Cooldown),
				new ActionTreeNode<float>(AnimateTakeDamage),
				new ActionTreeNode<System.Object>(Revive),
				new ActionTreeNode<float>(AnimateRevive),
				new ActionTreeNode<System.Object>(StartCooldown)
			}),
            new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
        );
    }
}