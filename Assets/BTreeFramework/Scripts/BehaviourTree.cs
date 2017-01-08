using System;
using UnityEngine;
using BTree;
using System.Collections.Generic;
using UniRx;

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
					if (_state != value) {
						_state = value;
						observable.OnNext(_state);
					}
				}
				get { return _state; } 
			}
			protected Subject<State> observable = new Subject<State>();

            private int executionCount = 0;
            private int executionLimit = 1;

			private State _state;

            public bool IsComplete()
            {
				return State == State.TERMINATED || State == State.SUCCESS || State == State.FAILURE;
            }

            public virtual void Tick(BehaviourTree tree)
            {
				if (tree.locked) 
				{
					State = State.TERMINATED;
					return;
				} 
				else if (executionCount == executionLimit)
                {
                    return;
                } 
				else if (State != State.RUNNING)
                {
                    State = State.WAITING;
                }
                tree.Queue(this);
				Execute(tree);
                if (IsComplete())
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

            public abstract Node[] GetChildren();
        }

        private Node rootNode;
        private Stack<Node> NodeQueue = new Stack<Node>();
		private bool locked;

        public BehaviourTree (Node rootNode, GameObject actor)
		{
			this.rootNode = rootNode;
		}

        public Node getRootNode()
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
                if (node.GetChildren().Length > 0)
                {
                    foreach (Node child in node.GetChildren())
                    {
                        if (!child.IsComplete())
                        {
                            return child;
                        }
                    }
                }
                else if (!node.IsComplete())
                {
                    return node;
                }
            }
            return rootNode;
        }
	}
}

