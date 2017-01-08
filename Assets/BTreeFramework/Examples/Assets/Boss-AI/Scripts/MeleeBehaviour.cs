using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using Steer2D;

[RequireComponent(typeof(EnemyAIActor))]
[RequireComponent(typeof(BTreeSelectorGroup))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Seek))]
[AddComponentMenu("AI Behaviour Tree/Melee Attack")]
public class MeleeBehaviour : AbstractBTreeBehaviour
{
    public float meleeDelay = 0.1f;
    public float meleeTime = 0.1f;
    public float meleeRadius;
    public float attackRadius;
    
	private EnemyAIActor actor;
    private Seek seek;

    protected override void Start()
    {
        base.Start();
		actor = gameObject.GetComponent<EnemyAIActor>();
        seek = gameObject.GetComponent<Seek>();
    }

    private float setLookAtX(Vector3 target)
    {
        float lookX = target.x - transform.position.x;
        if (lookX < 0)
        {
            animationController.LookLeft();
        }
        else
        {
            animationController.LookRight();
        }
        return lookX;
    }

    public bool IsTargetInRange()
    {
		return actor.GetTarget() != null;
    }

    public bool IsTargetInMeleeRange()
    {
		if (actor.GetTarget() != null && (actor.GetTarget().transform.position - transform.position).magnitude <= meleeRadius)
        {
            return true;
        }
        return false;
    }

    public bool IsTargetInMeleeAttackRange()
    {
		if (actor.GetTarget() != null && (actor.GetTarget().transform.position - transform.position).magnitude <= attackRadius)
        {
            return true;
        }
        return false;
    }

    public BehaviourTree.State Idle(BehaviourTreeNode<System.Object> node)
    {
        animationController.Idle();
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State Wake(BehaviourTreeNode<System.Object> node)
    {
        animationController.Wake();
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State MoveTowardsTarget(BehaviourTreeNode<System.Object> node)
    {
		if (actor.GetTarget() == null)
        {
            seek.TargetPoint = transform.position;
            animationController.Halt();
            return BehaviourTree.State.FAILURE;
        }
		seek.TargetPoint = actor.GetTarget().transform.position;
        animationController.Move();
		setLookAtX(actor.GetTarget().transform.position);
        if (IsTargetInMeleeAttackRange())
        {
            seek.TargetPoint = transform.position;
            animationController.Halt();
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State PrepareMeleeAttack(BehaviourTreeNode<float> node)
    {
		if (actor.GetTarget() == null)
        {
            animationController.WithdrawMeleeAttack();
            return BehaviourTree.State.FAILURE;
        }
        animationController.PrepareMeleeAttack();
		setLookAtX(actor.GetTarget().transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > meleeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State MeleeAttack(BehaviourTreeNode<float> node)
    {
        animationController.MeleeAttack();
        node.Result += Time.deltaTime;
        if (node.Result > meleeTime)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node)
    {
        animationController.WithdrawMeleeAttack();
        return BehaviourTree.State.SUCCESS;
    }

    protected override BehaviourTree.Node Initialize()
    {
        return new BinaryTreeNode(
            IsTargetInRange,
            new BinaryTreeNode(
                IsTargetInMeleeRange,
                new SequenceTreeNode(new BehaviourTree.Node[] {
                    new ActionTreeNode<System.Object>(Wake),
                    new ActionTreeNode<System.Object>(MoveTowardsTarget),
                    new BinaryTreeNode(
                        IsTargetInMeleeAttackRange,
                        new SequenceTreeNode(new BehaviourTree.Node[] {
                            new ActionTreeNode<float>(PrepareMeleeAttack),
                            new ActionTreeNode<float>(MeleeAttack)
                        }),
                        new ActionTreeNode<System.Object>(WithdrawAttack)
                    )
                }),
                new SequenceTreeNode(new BehaviourTree.Node[] {
                    new ActionTreeNode<System.Object>(WithdrawAttack),
                    new ActionTreeNode<System.Object>(Idle),
                    new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
                })
            ),
            new SequenceTreeNode(new BehaviourTree.Node[]
            {
                new ActionTreeNode<System.Object>(WithdrawAttack),
                new ActionTreeNode<System.Object>(Idle)
            })
        );
    }
}
