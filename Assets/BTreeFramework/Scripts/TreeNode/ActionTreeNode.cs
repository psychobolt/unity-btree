using System;
using UniRx;
using UnityEngine;

namespace BTree
{
	public class ActionTreeNode<T>: BehaviourTreeNode<T>
	{
		public delegate void Callback();

		protected Func<BehaviourTreeNode<T>, BehaviourTree.State> Action;

        protected ActionTreeNode () : base(new BehaviourTree.Node[] { })
        {
            this.Action = (node) =>
            {
                return BehaviourTree.State.SUCCESS;
            };
        }

        public ActionTreeNode (Callback action) : base(new BehaviourTree.Node[] { })
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

		public ActionTreeNode (Func<BehaviourTreeNode<T>, BehaviourTree.State> action) : base(new BehaviourTree.Node[] { })
		{
			this.Action = action;
		}

		protected override void Execute(BehaviourTree tree)
        {
            State = Action.Invoke(this);
		}

        protected override void OnExecute(BehaviourTree.Node child)
        {
        }

        public override BehaviourTree.Node[] GetNextChildren()
        {
            return children;
        }
    }

    public class ObservableActionTreeNode<T> : ActionTreeNode<T>
    {
        public ObservableActionTreeNode(Func<BehaviourTreeNode<T>, IObservable<BehaviourTree.State>> action) : base()
        {
            this.Action = (node) =>
            {
                if (State == BehaviourTree.State.WAITING)
                {
                    IObservable<BehaviourTree.State> observable = action.Invoke(this);
                    stream = GetStream().Merge(observable);
                    return BehaviourTree.State.RUNNING;
                }
                return State;
            };
        }
    }
}

