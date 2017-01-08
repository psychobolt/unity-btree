using System;
using UniRx;

namespace BTree
{
    public class RepeatTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node[] children;

        public RepeatTreeNode(BehaviourTree.Node node)
        {
            children = new BehaviourTree.Node[] { node };
			node.OnExecute().Subscribe(State => this.State = node.State);
        }

        public override void Tick(BehaviourTree tree)
        {
            setExecutionCount(this, 0);
            base.Tick(tree);
        }

        protected override void Execute(BehaviourTree tree)
        {
			foreach (BehaviourTree.Node child in children[0].GetChildren())
            {
                setExecutionCount(child, 0);
            }
			children[0].Tick(tree);
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
