using System;
using UniRx;

namespace BTree
{
    public class RandomTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node[] children;
        private Random random;
        private BehaviourTree.Node currentChild;

        public RandomTreeNode(BehaviourTree.Node[] children)
        {
            this.children = children;
			foreach (BehaviourTree.Node child in children) {
				child.OnExecute().Subscribe(State => this.State = State);
			}
            random = new Random();
            currentChild = children[random.Next(children.Length)];
        }

        protected override void Execute(BehaviourTree tree)
        {
            if (currentChild.IsComplete())
            {
                currentChild = children[random.Next(children.Length)];
            }
            currentChild.Tick(tree);
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { currentChild };
        }
    }
}
