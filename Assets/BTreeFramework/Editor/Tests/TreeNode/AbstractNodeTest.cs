using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace BTree 
{
	public abstract class AbstractNodeTest 
	{
		protected GameObject gameObject;

		protected const string TEST_STRING = "test";

		[SetUp]
		public void Setup() 
		{
			gameObject = new GameObject();
		}
			
		[Test]
		public void InitTest() 
		{
			BehaviourTree.State state = BehaviourTree.State.TERMINATED;
			ActionTreeNode<string> child = CreateImmediateAction(state);
			Assert.AreEqual(BehaviourTree.State.INITIALIZED, child.GetState());
		}

		protected void RunCounter(BehaviourTree btree, int max) 
		{
			for (int i = 1; i <= max; i++) 
			{
				btree.Tick();
				if (i < max) {
					Assert.AreEqual(BehaviourTree.State.RUNNING, btree.GetRootNode().GetState());
				}
			}
		}

		protected ActionTreeNode<string> CreateImmediateAction(BehaviourTree.State returnState) 
		{
			return new ActionTreeNode<string>(node => 
			{
				node.Result = TEST_STRING;
				return returnState;
			});
		}

		protected ActionTreeNode<int> CreateCounterAction(int max)
		{
			return new ActionTreeNode<int>(node => {
				int next = node.Result + 1;
				if (next >= max) {
					node.Result = max;
					return BehaviourTree.State.SUCCESS;
				}
				node.Result = next;
				return BehaviourTree.State.RUNNING;
			});
		}
	}
}
