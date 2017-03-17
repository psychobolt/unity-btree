using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BTree
{
    public class BehaviourTree
	{
        public enum State
        {
			INITIALIZED,
            WAITING,
            SUCCESS,
            FAILURE,
            RUNNING,
			TERMINATED
        }

        public abstract class Node
        {
			protected internal State State { 
				set {
                    _state = value;
                    observable.OnNext(_state);
                }
				get { return _state; } 
			}
            protected Node[] children;
			protected Subject<State> observable = new Subject<State>();

            private int executionCount = 0;
            private int executionLimit = 1;

			private State _state;

            protected Node(Node[] children)
            {
                this.children = children;
                foreach (Node child in children)
                {
                    child.OnExecute().Subscribe(state => OnExecute(child));
                }
            }

			public State GetState() {
				return _state;
			}
            
            public Node[] GetChildren()
            {
                return children;
            }

            public bool IsTerminated()
            {
				return executionCount == executionLimit || State == State.TERMINATED || State == State.SUCCESS || State == State.FAILURE;
            }

            protected internal virtual void Tick(BehaviourTree tree)
            {
				if (tree.locked) 
				{
					State = State.TERMINATED;
					return;
				} 
				else if (executionCount == executionLimit)
                {
					observable.OnNext(_state);
                    return;
                } 
				else if (State == State.INITIALIZED || IsTerminated())
                {
                    State = State.WAITING;
                }
                tree.Queue(this);
				Execute(tree);
                if (IsTerminated())
                {
                    executionCount++;
                    tree.RemoveQueue();
                }
            }

			public IObservable<State> OnExecute() {
				return observable;
			}

            protected void setExecutionCount(Node node, int count)
            {
                node.executionCount = count;
                foreach (Node child in node.GetChildren())
                {
                    setExecutionCount(child, 0);
                }
            }
            
            protected abstract void Execute(BehaviourTree tree);

            protected abstract void OnExecute(Node child);

            public abstract Node[] GetNextChildren();
        }

        private Node rootNode;
        private Stack<Node> NodeQueue = new Stack<Node>();
		private bool locked;

        public BehaviourTree (Node rootNode, GameObject actor)
		{
			this.rootNode = rootNode;
		}

        public Node GetRootNode()
        {
            return rootNode;
        }

		public void Tick()
        {
            Node current = GetCurrentNode(rootNode);
            current.Tick(this);
        }

        public void Queue(Node node)
        {
            NodeQueue.Push(node);
        }

        public Node RemoveQueue()
        {
            Node node = NodeQueue.Pop();
            return node;
        }

		public void Lock()
		{
			locked = true;
		}

		public void Unlock() {
			locked = false;
		}

        private Node GetCurrentNode(Node node)
        {
            while(NodeQueue.Count > 0)
            {
                node = RemoveQueue();
                if (node.GetNextChildren().Length > 0)
                {
                    return node.GetNextChildren().First();
                }
                else if (!node.IsTerminated())
                {
                    return node;
                }
            }
            return rootNode;
        }
	}
}

