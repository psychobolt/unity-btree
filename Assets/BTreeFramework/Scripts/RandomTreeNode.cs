using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            State = currentChild.State;
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { currentChild };
        }
    }
}
