using NUnit.Framework;
using UnityEditor;

namespace BTree
{
	public class SelectorTreeNodeTest : AbstractNodeTest
	{
		[Test]
		public void ExecuteNoChildrenTest()
		{
			SelectorTreeNode selector = new SelectorTreeNode(new BehaviourTree.Node[] {});
			BehaviourTree btree = new BehaviourTree(selector, gameObject);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.WAITING, selector.GetState());
		}

		[Test]
		public void ExecuteAllTest() 
		{
			int max = 10;
			SelectorTreeNode selector = new SelectorTreeNode(new BehaviourTree.Node[] {
				CreateImmediateAction(BehaviourTree.State.FAILURE),
				CreateCounterAction(max)
			});
			BehaviourTree btree = new BehaviourTree(selector, gameObject);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.RUNNING, selector.GetState());
			RunCounter(btree, max - 1);
			Assert.AreEqual(BehaviourTree.State.SUCCESS, selector.GetState());
		}

		[Test]
		public void ExecuteSomeTest()
		{
			SelectorTreeNode selector = new SelectorTreeNode(new BehaviourTree.Node[] {
				CreateImmediateAction(BehaviourTree.State.FAILURE),
				CreateImmediateAction(BehaviourTree.State.SUCCESS),
				CreateImmediateAction(BehaviourTree.State.FAILURE)
			});
			BehaviourTree btree = new BehaviourTree(selector, gameObject);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.SUCCESS, selector.GetState());
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.SUCCESS, selector.GetState());
		}
	}
}

