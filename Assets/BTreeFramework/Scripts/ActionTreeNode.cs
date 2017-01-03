using System;
using UnityEngine;

namespace BTree
{
	public class ActionTreeNode<T>: BehaviourTreeNode<T>
	{
		private Func<BehaviourTreeNode<T>, BehaviourTree.State> Action;

        public delegate void Callback();

        public ActionTreeNode (Callback action)
        {
            this.Action = (node) =>
            {
                try
                {
                    action();
                    return BehaviourTree.State.SUCCESS;
                }
                catch (Exception e)
                {
                    // TODO hide log in build
                    Debug.LogWarning("[BTreeFramework]: Exception on executing a action");
                    Debug.LogException(e);
                    return BehaviourTree.State.FAILURE;
                }
            };
        }

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

