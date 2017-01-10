using System;
using System.Linq;
using UniRx;

namespace BTree
{
    class SelectorTreeNode : BehaviourTree.Node
    {
        public SelectorTreeNode(BehaviourTree.Node[] children) : base(children)
        {
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
				else if (!child.IsTerminated())
                {
                    return;
                }
            }
        }

        public override BehaviourTree.Node[] GetNextChildren()
        {
            return children.Where(child => !child.IsTerminated()).ToArray();
        }

        protected override void OnExecute(BehaviourTree.Node child)
        {
            if (child.State != BehaviourTree.State.FAILURE || Array.IndexOf(children, child) == children.Length - 1)
            {
                this.State = child.State;
            }
        }
    }
}
