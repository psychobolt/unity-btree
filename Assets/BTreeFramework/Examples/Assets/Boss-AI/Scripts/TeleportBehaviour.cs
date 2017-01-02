
using System;
using System.Collections.Generic;
using BTree;
using UnityEngine;

[RequireComponent(typeof(BTreeStandaloneGroup))]
[AddComponentMenu("AI Behaviour Tree/Teleport")]
public class TeleportBehaviour : AbstractBTreeBehaviour
{
    public override BehaviourTree.Node GetBehaviourTree()
    {
        return new ActionTreeNode<System.Object>(node => BehaviourTree.State.SUCCESS);
    }
}