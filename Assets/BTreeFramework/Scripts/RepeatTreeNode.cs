using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
    class RepeatTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node node;

        private BehaviourTree.Node[] children;

        public RepeatTreeNode(BehaviourTree.Node node)
        {
            this.node = node;
            children = new BehaviourTree.Node[] { node };
        }

        public override void Tick(BehaviourTree tree)
        {
            setExecutionCount(this, 0);
            base.Tick(tree);
        }

        protected override void Execute(BehaviourTree tree)
        {
            foreach (BehaviourTree.Node child in node.GetChildren())
            {
                setExecutionCount(child, 0);
            }
            node.Tick(tree);
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
