using System;
using System.Linq;

namespace BTree
{
    public class RandomTreeNode : BehaviourTree.Node
    {
        private Random random;
        private BehaviourTree.Node currentChild;

        public RandomTreeNode(BehaviourTree.Node[] children) : base(children)
        {
            random = new Random();
            currentChild = children[random.Next(children.Length)];
        }

        protected override void Execute(BehaviourTree tree)
        {
            if (currentChild.IsTerminated())
            {
                currentChild = children[random.Next(children.Length)];
            }
            currentChild.Tick(tree);
        }

        public override BehaviourTree.Node[] GetNextChildren()
        {
            if (currentChild.IsTerminated())
            {
                return new BehaviourTree.Node[] { };
            }
            return new BehaviourTree.Node[] { currentChild };
        }

        protected override void OnExecute(BehaviourTree.Node child)
        {
            this.State = child.State;
        }
    }
}
