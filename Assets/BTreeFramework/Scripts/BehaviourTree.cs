using System;
using UnityEngine;
using BTree;
using System.Collections.Generic;

namespace BTree
{
	public class BehaviourTree
	{
        public enum State
        {
            WAITING,
            SUCCESS,
            FAILURE,
            RUNNING
        }

        public abstract class Node
        {
            public State State { set; get; }

            private int executionCount = 0;
            private int executionLimit = 1;

            public bool IsComplete()
            {
                return State == State.SUCCESS || State == State.FAILURE;
            }

            public virtual void Tick(BehaviourTree tree)
            {
                if (executionCount == executionLimit)
                {
                    return;
                }
                if (State != State.RUNNING)
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

        public BehaviourTree (Node rootNode, GameObject actor)
		{
			this.rootNode = rootNode;
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

