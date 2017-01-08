using System;
using System.Collections.Generic;
using UniRx;

namespace BTree
{
    public class SequenceTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node[] children;

        public SequenceTreeNode(BehaviourTree.Node[] children)
        {
            this.children = children;
			foreach (BehaviourTree.Node child in children) {
				child.OnExecute().Subscribe(state => {
					if (state != BehaviourTree.State.SUCCESS ||
						Array.IndexOf(children, child) == children.Length - 1)
					{
						State = state;
					}
				});
			}
        }

        protected override void Execute(BehaviourTree tree)
        {
            foreach (BehaviourTree.Node child in children)
            {
                child.Tick(tree);
                if (!child.IsComplete())
                {
                    return;
                } 
				else if (child.State != BehaviourTree.State.SUCCESS)
				{
					break;
				}
            }
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
