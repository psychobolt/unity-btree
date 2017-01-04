using BTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BTreeSequenceGroup : AbstractBTreeBehaviour
{
    public string groupName;

    private BehaviourTree btree;

    // Update is called once per frame
    void Update()
    {
        if (string.IsNullOrEmpty(parent))
        {
            GetBehaviourTree();
            btree.Tick();
        }
    }
    
    public override BehaviourTree.Node GetBehaviourTree()
    {
        if (btree == null)
        {
            List<BehaviourTree.Node> behaviours = new List<BehaviourTree.Node>();
            char[] delimiters = new[] { ';' };
            foreach (Component component in gameObject.GetComponents(typeof(AbstractBTreeBehaviour)))
            {
                AbstractBTreeBehaviour behaviour = (AbstractBTreeBehaviour)component;
                if (behaviour.enabled && Array.Exists(behaviour.parent.Split(delimiters), parent => parent == groupName))
                {
                    behaviours.Add(behaviour.GetBehaviourTree());
                }
            }
            btree = new BehaviourTree(new RepeatTreeNode(new SequenceTreeNode(behaviours.ToArray())), gameObject);
        }
        return btree.getRootNode();
    }
}
