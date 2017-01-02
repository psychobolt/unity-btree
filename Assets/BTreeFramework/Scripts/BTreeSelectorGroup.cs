using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using System;

public class BTreeSelectorGroup : AbstractBTreeBehaviour
{
    public string groupName;

    private BehaviourTree btree;
	
	// Update is called once per frame
	void Update ()
    {
        if (string.IsNullOrEmpty(parent))
        {
            btree.Tick();
        }
    }

    public override BehaviourTree.Node GetBehaviourTree()
    {
        if (btree == null)
        {
            List<BehaviourTree.Node> behaviours = new List<BehaviourTree.Node>();
            foreach (Component component in gameObject.GetComponents(typeof(AbstractBTreeBehaviour)))
            {
                AbstractBTreeBehaviour behaviour = (AbstractBTreeBehaviour)component;
                if (groupName == behaviour.parent)
                {
                    behaviours.Add(behaviour.GetBehaviourTree());
                }
            }
            btree = new BehaviourTree(new RepeatTreeNode(new SelectorTreeNode(behaviours.ToArray())), gameObject);
        }
        return btree.getRootNode();
    }
}
