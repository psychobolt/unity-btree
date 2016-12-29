using System;

namespace BTree
{
    public abstract class BehaviourTreeNode<T> : BehaviourTree.Node
	{
        public T Result;
	}
}

