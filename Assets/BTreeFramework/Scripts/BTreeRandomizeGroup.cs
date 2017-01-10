using System;
using System.Collections.Generic;

namespace BTree
{
    public class BTreeRandomizeGroup : AbstractBTreeGroup
    {
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
}
