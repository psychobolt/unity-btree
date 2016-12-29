using System;

namespace BTree
{
	public class ActionTreeNode<T>: BehaviourTreeNode<T>
	{
		public Func<BehaviourTreeNode<T>, BehaviourTree.State> Action;

		public ActionTreeNode (Func<BehaviourTreeNode<T>, BehaviourTree.State> action)
		{
			this.Action = action;
		}

		protected override void Execute(BehaviourTree tree)
        {
            State = Action.Invoke(this);
		}

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { };
        }
    }
}

