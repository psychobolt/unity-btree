using System;
using System.Collections.Generic;
using UniRx;

namespace BTree
{
    class SelectorTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node[] children;

        public SelectorTreeNode(BehaviourTree.Node[] children) {
            this.children = children;
			foreach (BehaviourTree.Node child in children) {
				child.OnExecute().Subscribe(state => { 
					if (state != BehaviourTree.State.FAILURE || 
						Array.IndexOf(children, child) == children.Length - 1) {
						this.State = state;
					}
				});
			}
        }

        protected override void Execute(BehaviourTree tree)
        {
            foreach(BehaviourTree.Node child in children)
            {
                child.Tick(tree);
				if (child.State == BehaviourTree.State.TERMINATED || child.State == BehaviourTree.State.SUCCESS)
                {
                    break;
                } 
				else if (!child.IsComplete())
                {
                    return;
                }
            }
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
