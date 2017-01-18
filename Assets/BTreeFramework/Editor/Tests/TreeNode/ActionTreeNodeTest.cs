using NUnit.Framework;
using UnityEditor;

namespace BTree 
{
	public class ActionTreeNodeTest : AbstractNodeTest 
	{
		[Test]
		public void ActionSuccessTest() 
		{
			BehaviourTree.State state = BehaviourTree.State.SUCCESS;
			ActionTreeNode<string> child = CreateImmediateAction(state);
			BehaviourTree btree = new BehaviourTree(child, gameObject);
			btree.Tick();
			Assert.AreSame(TEST_STRING, child.Result);
			Assert.AreEqual(state, btree.GetRootNode().GetState());
			Assert.IsTrue(child.IsTerminated());
		}

		[Test]
		public void ActionFailedTest() 
		{
			BehaviourTree.State state = BehaviourTree.State.FAILURE;
			ActionTreeNode<string> child = CreateImmediateAction(state);
			BehaviourTree btree = new BehaviourTree(child, gameObject);
			btree.Tick();
			Assert.AreSame(TEST_STRING, child.Result);
			Assert.AreEqual(state, child.GetState());
			Assert.IsTrue(child.IsTerminated());
		}

		[Test]
		public void ActionRunningTest() 
		{
			BehaviourTree.State state = BehaviourTree.State.RUNNING;
			ActionTreeNode<string> child = CreateImmediateAction(state);
			BehaviourTree btree = new BehaviourTree(child, gameObject);
			btree.Tick();
			Assert.AreSame(TEST_STRING, child.Result);
			Assert.AreEqual(state, child.GetState());
			Assert.IsFalse(child.IsTerminated());
		}
	}
}
