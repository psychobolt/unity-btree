using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Linq;

namespace BTree
{
    [AddComponentMenu("")]
    public class BTreeTickBehaviour : ObservableTriggerBase
    {
        private List<BehaviourTree> btrees;
        private Subject<Dictionary<string, BehaviourTree>> groupsAsObservable = new Subject<Dictionary<string, BehaviourTree>>();

        void Start()
        {
            btrees = new List<BehaviourTree>();
            Dictionary<string, BehaviourTree> groups = new Dictionary<string, BehaviourTree>();
            foreach (AbstractBTreeBehaviour behaviour in gameObject.GetComponents<AbstractBTreeBehaviour>())
            {
				if (behaviour.enabled && (behaviour.parents.Length == 0 || behaviour.parents.Contains("")))
                {
                    BehaviourTree btree = new BehaviourTree(new RepeatTreeNode(behaviour.GetBehaviourTree()), gameObject);
                    btrees.Add(btree);
                    if (behaviour is AbstractBTreeGroup)
                    {
                        AbstractBTreeGroup group = (AbstractBTreeGroup)behaviour;
                        groups.Add(group.groupName, btree);
                    }
                }
            }
            groupsAsObservable.OnNext(groups);
            this.UpdateAsObservable().Subscribe(x =>
            {
				foreach (BehaviourTree btree in btrees)
                {
					btree.Tick();
                }
            });
        }

        public IObservable<Dictionary<string, BehaviourTree>> GroupsAsObservable()
        {
            return groupsAsObservable;
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            groupsAsObservable.OnCompleted();
        }
    }
}
