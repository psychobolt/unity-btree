using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
    public class SequenceTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node[] children;

        public SequenceTreeNode(BehaviourTree.Node[] children)
        {
            this.children = children;
        }

        protected override void Execute(BehaviourTree tree)
        {
            foreach (BehaviourTree.Node child in children)
            {
                child.Tick(tree);
                if (child.State == BehaviourTree.State.FAILURE)
                {
                    State = child.State;
                    return;
                }
                else if (!child.IsComplete())
                {
                    return;
                }
            }
            State = BehaviourTree.State.SUCCESS;
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
