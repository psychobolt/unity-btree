using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("AI Behaviour Tree/Range Attack")]
public class RangeBehaviour : AbstractBTreeBehaviour
{
    public float rangeRadius;
    public float rangeDelay = 0.1f;
    public float rangeTime = 0.1f;
    public string rangeBehaviorName;
    private MonoBehaviour rangeBehavior;

    private Animator animator;
    private GameObject target;

    void Start ()
    {
        animator = gameObject.GetComponent<Animator>();
        InitExternalBehaviors();
    }

    private void InitExternalBehaviors()
    {
        foreach (Component component in gameObject.GetComponents(typeof(MonoBehaviour)))
        {
            MonoBehaviour behavior = (MonoBehaviour)component;
            if (component.GetType().ToString() == rangeBehaviorName)
            {
                rangeBehavior = behavior;
                rangeBehavior.enabled = false;
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
        animator.SetFloat("lookX", lookX > 0 ? 1 : -1);
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
        animator.SetBool("idle", true);
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State Wake(BehaviourTreeNode<System.Object> node)
    {
        animator.SetBool("idle", false);
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
            animator.SetBool("range", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.FAILURE;
        }
        animator.SetBool("range", true);
        animator.SetBool("prepare", true);
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
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
        node.Result += Time.deltaTime;
        if (rangeBehavior)
        {
            rangeBehavior.enabled = true;
        }
        if (node.Result > rangeTime)
        {
            node.Result = 0;
            if (rangeBehavior)
            {
                rangeBehavior.enabled = false;
            }
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node)
    {
        animator.SetBool("prepare", false);
        animator.SetBool("range", false);
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
