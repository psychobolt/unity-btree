
using System;
using System.Collections.Generic;
using BTree;
using UnityEngine;

[RequireComponent(typeof(EnemyAIActor))]
[RequireComponent(typeof(BTreeSelectorGroup))]
[AddComponentMenu("AI Behaviour Tree/Teleport")]
public class TeleportBehaviour : AbstractBTreeBehaviour
{
    public float duration = 1.0f;
    public float delay = 5.0f;
    
    private EnemyAIActor actor;

    protected override void Start()
    {
        base.Start();
        actor = gameObject.GetComponent<EnemyAIActor>();
    }

    public BehaviourTree.State Teleport(BehaviourTreeNode<float> node)
    {
        if (node.Result == 0)
        {
            animationController.Disappear();
        }
        node.Result += Time.deltaTime;
        if (node.Result > duration)
        {
            node.Result = 0;
            animationController.Appear();
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public override BehaviourTree.Node GetBehaviourTree()
    {
        return new BinaryTreeNode(
            actor.IsLowHealth,
            new SequenceTreeNode(new BehaviourTree.Node[]
            {
                new ActionTreeNode<float>(Teleport),
                new ActionTreeNode<System.Object>(actor.Revive)
            }),
            new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
        );
    }
}