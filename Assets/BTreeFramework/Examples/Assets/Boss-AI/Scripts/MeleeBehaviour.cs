using System;
using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
using Steer2D;

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

    private Animator animator;
    private GameObject target;
    private Seek seek;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        seek = gameObject.GetComponent<Seek>();
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

    public bool IsTargetInMeleeRange()
    {
        if (target != null && (target.transform.position - transform.position).magnitude <= meleeRadius)
        {
            Debug.Log("Death is near for you!");
            return true;
        }
        return false;
    }

    public bool IsTargetInMeleeAttackRange()
    {
        if (target != null && (target.transform.position - transform.position).magnitude <= attackRadius)
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

    public BehaviourTree.State MoveTowardsTarget(BehaviourTreeNode<System.Object> node)
    {
        if (target == null)
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.FAILURE;
        }
        seek.TargetPoint = target.transform.position;
        animator.SetBool("move", true);
        setLookAtX(target.transform.position);
        if (IsTargetInMeleeAttackRange())
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State PrepareMeleeAttack(BehaviourTreeNode<float> node)
    {
        if (target == null)
        {
            animator.SetBool("melee", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.FAILURE;
        }
        animator.SetBool("melee", true);
        animator.SetBool("prepare", true);
        setLookAtX(target.transform.position);
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
        animator.SetBool("melee", true);
        animator.SetBool("prepare", false);
        node.Result += Time.deltaTime;
        if (node.Result > meleeTime)
        {
            Debug.Log("Muahahaha!");
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node)
    {
        animator.SetBool("prepare", false);
        animator.SetBool("melee", false);
        return BehaviourTree.State.SUCCESS;
    }

    public override BehaviourTree.Node GetBehaviourTree()
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
