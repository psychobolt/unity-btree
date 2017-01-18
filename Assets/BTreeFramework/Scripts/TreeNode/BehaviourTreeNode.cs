using UniRx;

namespace BTree
{
    public abstract class BehaviourTreeNode<T> : BehaviourTree.Node
	{
        public T Result;

		protected IObservable<BehaviourTree.State> stream;

		public IObservable<BehaviourTree.State> GetStream() {
			if (stream == null) {
				Subject<BehaviourTree.State> stream = new Subject<BehaviourTree.State>();
				stream.Subscribe(state => State = state);
				this.stream = stream;
			}
			return stream;
		}

        protected BehaviourTreeNode(BehaviourTree.Node[] children) : base(children)
        {
			
        }
    }
}

