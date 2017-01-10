namespace BTree
{
    public abstract class BehaviourTreeNode<T> : BehaviourTree.Node
	{
        public T Result;

        protected BehaviourTreeNode(BehaviourTree.Node[] children) : base(children)
        {
        }
    }
}

