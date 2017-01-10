
using System;
using System.Collections.Generic;
using BTree;
using UnityEngine;

[RequireComponent(typeof(EnemyAIActor))]
[RequireComponent(typeof(BTreeSelectorGroup))]
[AddComponentMenu("AI Behaviour Tree/Take Damage")]
public class TakeDamageBehaviour : AbstractBTreeBehaviour
{
    public float disappearAnimationTime = 1.0f;
    public float appearAnimationTime = 1.0f;
    public int hitPercentage = 100;
    public int revivePrecentage = 100;

    private EnemyAIActor actor;

    protected override void Start()
    {
        base.Start();
        actor = gameObject.GetComponent<EnemyAIActor>();
    }
    
    public bool IsLowHealth()
    {
        return ((actor.totalHP - actor.health) / actor.totalHP) * 100 >= hitPercentage;
    }

    public BehaviourTree.State Disappear(BehaviourTreeNode<float> node)
    {
        if (disappearAnimationTime <= 0)
        {
            return BehaviourTree.State.SUCCESS;
        }
        if (node.Result == 0)
        {
            animationController.Disappear();
        }
        node.Result += Time.deltaTime;
        if (node.Result > disappearAnimationTime)
        {
			node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State Appear(BehaviourTreeNode<float> node)
    {
        if (appearAnimationTime <= 0)
        {
            return BehaviourTree.State.SUCCESS;
        }
        if (node.Result == 0)
        {
            animationController.Appear();
        }
        node.Result += Time.deltaTime;
        if (node.Result > appearAnimationTime)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(0f, 0f, 0f);
    }

    private void Revive()
    {
        actor.health += (actor.totalHP - actor.health) * (revivePrecentage / 100);
    }

    protected override BehaviourTree.Node Initialize()
    {
        return new BinaryTreeNode(
            IsLowHealth,
            new SequenceTreeNode(new BehaviourTree.Node[]
            {
                new ActionTreeNode<float>(Disappear),
                new ActionTreeNode<float>(Appear),
				new ActionTreeNode<System.Object>(Revive)
            }),
            new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
        );
    }
}