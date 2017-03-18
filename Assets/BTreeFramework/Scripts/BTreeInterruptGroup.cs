using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace BTree
{
    public class BTreeInterruptGroup : ObservableTriggerBase
    {

        public string groupName;
        public string[] interuptRootGroups = new string[] { };
        public BehaviourTree.State[] interruptOnStates;

        private List<AbstractBTreeBehaviour> children;
        private HashSet<string> interruptGroups;
        private BTreeTickBehaviour tickBehaviour;
        private IDisposable subscription;

        void Start()
        {
            children = new List<AbstractBTreeBehaviour>();
            interruptGroups = new HashSet<string>();
            tickBehaviour = gameObject.GetComponent<BTreeTickBehaviour>();
            if (!tickBehaviour)
            {
                tickBehaviour = gameObject.AddComponent<BTreeTickBehaviour>();
            }
            AbstractBTreeBehaviour[] behaviours = gameObject.GetComponents<AbstractBTreeBehaviour>();
            foreach (AbstractBTreeBehaviour behaviour in behaviours)
            {
                if (behaviour.enabled)
                {
                    if (Array.Exists(behaviour.parents, parent => parent == groupName))
                    {
                        children.Add(behaviour);
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
            }
            tickBehaviour.GroupsAsObservable().Subscribe(groups =>
            {
                BehaviourTree[] interruptTrees = groups.Where(keyPair => interruptGroups.Contains(keyPair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value).Values.ToArray();
                subscription = Observable.ToObservable(children).SelectMany(behaviour => Observable.AsObservable(behaviour.GetRoot().GetBehaviourTree().OnExecute())).BatchFrame().Subscribe(states =>
                {
                    if (states.Any(state => interruptOnStates.Contains(state)))
                    {
                        foreach (BehaviourTree tree in interruptTrees)
                        {
                            tree.Lock();
                        }
                    }
                    else if (states.Any(state => !interruptOnStates.Contains(BehaviourTree.State.WAITING) && state != BehaviourTree.State.WAITING))
                    {
                        foreach (BehaviourTree tree in interruptTrees)
                        {
                            tree.Unlock();
                        }
                    }
                });
            });
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            subscription.Dispose();
        }
    }
}
