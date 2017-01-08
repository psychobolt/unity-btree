using BTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BTreeRandomizeGroup : AbstractBTreeGroup
{
	private BehaviourTree.Node rootNode;

    protected override BehaviourTree.Node Initialize()
    {
        List<BehaviourTree.Node> behaviours = new List<BehaviourTree.Node>();
		foreach (AbstractBTreeBehaviour behaviour in gameObject.GetComponents<AbstractBTreeBehaviour>())
        {
			if (behaviour.enabled && Array.Exists(behaviour.parents, parent => parent == groupName))
            {
				behaviours.Add(behaviour.GetBehaviourTree());
            }
        }
        return new RandomTreeNode(behaviours.ToArray());
    }
}
