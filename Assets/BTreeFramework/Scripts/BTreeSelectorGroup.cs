using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using System;

public class BTreeSelectorGroup : AbstractBTreeGroup
{
    private BehaviourTree.Node rootNode;

    protected override BehaviourTree.Node Initialize()
    {
        List<BehaviourTree.Node> behaviours = new List<BehaviourTree.Node>();
        foreach (Component component in gameObject.GetComponents(typeof(AbstractBTreeBehaviour)))
        {
            AbstractBTreeBehaviour behaviour = (AbstractBTreeBehaviour)component;
            if (behaviour.enabled && Array.Exists(behaviour.parents, parent => parent == groupName))
            {
                behaviours.Add(behaviour.GetBehaviourTree());
            }
        }
        return new SelectorTreeNode(behaviours.ToArray());
    }
}
