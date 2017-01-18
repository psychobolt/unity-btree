using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BTree
{
    public class BTreeInterruptBehaviour : MonoBehaviour
    {

        public string groupName;
        public string[] interuptRootGroups = new string[] { };
        public BehaviourTree.State[] interruptOnStates;

        private HashSet<string> interruptGroups;
        private BTreeTickBehaviour tickBehaviour;

        void Start()
        {
            interruptGroups = new HashSet<string>();
            tickBehaviour = gameObject.GetComponent<BTreeTickBehaviour>();
            if (!tickBehaviour)
            {
                tickBehaviour = gameObject.AddComponent<BTreeTickBehaviour>();
            }
            AbstractBTreeBehaviour[] behaviours = gameObject.GetComponents<AbstractBTreeBehaviour>();
            foreach (AbstractBTreeBehaviour behaviour in behaviours)
            {
                if (!behaviour.enabled)
                {
                    continue;
                }
                if (behaviour is AbstractBTreeGroup)
                {
                    AbstractBTreeGroup group = (AbstractBTreeGroup)behaviour;
                    if (Array.Exists(interuptRootGroups, root => root == group.groupName))
                    {
                        group = (AbstractBTreeGroup)behaviour.GetRoot();
                        interruptGroups.Add(group.groupName);
                    }
                }
            }
            tickBehaviour.GroupsAsObservable().Subscribe(groups =>
            {
                foreach (AbstractBTreeBehaviour behaviour in behaviours)
                {
                    if (!behaviour.enabled)
                    {
                        continue;
                    }
                    if (Array.Exists(behaviour.parents, parent => parent == groupName))
                    {
                        new InterruptTreeNode(
                            behaviour.GetRoot().GetBehaviourTree(),
                            interruptOnStates,
                            groups.Where(keyPair => interruptGroups.Contains(keyPair.Key))
                                .ToDictionary(pair => pair.Key, pair => pair.Value).Values.ToArray()
                       );
                    }
                }
            });
        }
    }

    class InterruptTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.State[] interruptOnStates;
        private BehaviourTree[] interruptTrees;

        public InterruptTreeNode(
            BehaviourTree.Node child, 
            BehaviourTree.State[] interruptOnStates, 
            BehaviourTree[] interruptTrees
            ) : base(new BehaviourTree.Node[] { child })
        {
            this.interruptOnStates = interruptOnStates;
            this.interruptTrees = interruptTrees;
        }

        public override BehaviourTree.Node[] GetNextChildren()
        {
            return children[0].IsTerminated() ? new BehaviourTree.Node[] { } : children;
        }

        protected override void Execute(BehaviourTree tree)
        {
			children[0].Tick(tree);
        }

        protected override void OnExecute(BehaviourTree.Node child)
        {
            if (interruptOnStates.Contains(child.State))
            {
                foreach (BehaviourTree tree in interruptTrees)
                {
                    tree.Lock();
                }
            }
            else if (!interruptOnStates.Contains(BehaviourTree.State.WAITING) && child.State != BehaviourTree.State.WAITING)
            {
                foreach (BehaviourTree tree in interruptTrees)
                {
                    tree.Unlock();
                }
            }
        }
    }
}
