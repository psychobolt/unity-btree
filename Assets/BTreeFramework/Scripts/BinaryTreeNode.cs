using System;

namespace BTree
{
	public class BinaryTreeNode : BehaviourTreeNode<bool>
	{
		private Func<bool> condition;
		private BehaviourTree.Node actionIfTrue;
		private BehaviourTree.Node actionIfFalse;

        public BinaryTreeNode (Func<bool> condition, BehaviourTree.Node actionIfTrue, BehaviourTree.Node actionIfFalse)
		{
			this.condition = condition;
			this.actionIfTrue = actionIfTrue;
			this.actionIfFalse = actionIfFalse;
        }

		protected override void Execute(BehaviourTree tree) {
            Result = condition.Invoke();
            BehaviourTree.Node action = Result ? actionIfTrue : actionIfFalse;
            action.Tick(tree);
            if (action.IsComplete())
            {
                State = action.State;
            }
		}

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { Result ? actionIfTrue : actionIfFalse };
        }
    }
}

