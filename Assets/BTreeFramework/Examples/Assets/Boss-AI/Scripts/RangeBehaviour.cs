using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("AI Behaviour Tree/Range Attack")]
public class RangeBehaviour : AbstractBTreeBehaviour
{
    public float rangeRadius;
    public float rangeDelay = 0.1f;
    public float rangeTime = 0.1f;
    public string projectileBehaviorName;
    private AbstractProjectileBehaviour projectileBehavior;
    
    private GameObject target;

    protected override void Start ()
    {
        base.Start();
        InitExternalBehaviors();
    }

    private void InitExternalBehaviors()
    {
        foreach (Component component in gameObject.GetComponents(typeof(AbstractProjectileBehaviour)))
        {
            AbstractProjectileBehaviour behavior = (AbstractProjectileBehaviour)component;
            if (component.GetType().ToString() == projectileBehaviorName)
            {
                projectileBehavior = behavior;
                projectileBehavior.enabled = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = null;
        }
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
        return target != null;
    }

    public bool IsTargetInRangeRadius()
    {
        if (target != null && (target.transform.position - transform.position).magnitude > rangeRadius)
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

    public BehaviourTree.State LookAtTarget(BehaviourTreeNode<System.Object> node)
    {
        if (target == null)
        {
            return BehaviourTree.State.SUCCESS;
        }
        setLookAtX(target.transform.position);
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State PrepareRangeAttack(BehaviourTreeNode<float> node)
    {
        if (target == null)
        {
            animationController.WithdrawRangeAttack();
            return BehaviourTree.State.FAILURE;
        }
        animationController.PrepareRangeAttack();
        setLookAtX(target.transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > rangeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State RangeAttack(BehaviourTreeNode<float> node)
    {
        animationController.RangeAttack();
        if (projectileBehavior && node.Result == 0f)
        {
            projectileBehavior.Fire();
        }
        node.Result += Time.deltaTime;
        if (node.Result > rangeTime)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node)
    {
        animationController.WithdrawRangeAttack();
        return BehaviourTree.State.SUCCESS;
    }

    public override BehaviourTree.Node GetBehaviourTree()
    {
        return new BinaryTreeNode(
            IsTargetInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(LookAtTarget),
                new BinaryTreeNode(
                    IsTargetInRangeRadius,
                    new SequenceTreeNode(new BehaviourTree.Node[] {
                        new ActionTreeNode<System.Object>(Wake),
                        new ActionTreeNode<float>(PrepareRangeAttack),
                        new ActionTreeNode<float>(RangeAttack),
                        new ActionTreeNode<System.Object>(WithdrawAttack)
                    }),
                    new ActionTreeNode<System.Object>(WithdrawAttack)
                )
            }),
            new SequenceTreeNode(new BehaviourTree.Node[]
            {
                new ActionTreeNode<System.Object>(WithdrawAttack),
                new ActionTreeNode<System.Object>(Idle)
            })
        );
    }
}
