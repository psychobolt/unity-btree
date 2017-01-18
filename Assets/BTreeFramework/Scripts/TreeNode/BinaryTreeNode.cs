using System;

namespace BTree
{
    public class BinaryTreeNode : BehaviourTreeNode<bool>
	{
		private Func<bool> condition;
		private BehaviourTree.Node actionIfTrue;
		private BehaviourTree.Node actionIfFalse;

        public BinaryTreeNode (Func<bool> condition, BehaviourTree.Node actionIfTrue, BehaviourTree.Node actionIfFalse) : base (new BehaviourTree.Node[] { actionIfTrue, actionIfFalse})
		{
			this.condition = condition;
			this.actionIfTrue = actionIfTrue;
			this.actionIfFalse = actionIfFalse;
        }

		protected override void Execute(BehaviourTree tree) {
            Result = condition.Invoke();
            BehaviourTree.Node action = Result ? actionIfTrue : actionIfFalse;
            action.Tick(tree);
		}

        public override BehaviourTree.Node[] GetNextChildren()
        {
            if (Result && !actionIfTrue.IsTerminated())
            {
                return new BehaviourTree.Node[] { actionIfTrue };
            }
			else if (!Result && !actionIfFalse.IsTerminated())
            {
                return new BehaviourTree.Node[] { actionIfFalse };
            }
            return new BehaviourTree.Node[] { };
        }

        protected override void OnExecute(BehaviourTree.Node child)
        {
            this.State = child.State;
        }
    }
}

