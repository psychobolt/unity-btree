using NUnit.Framework;
using UnityEditor;

namespace BTree 
{
	public class RepeatTreeNodeTest : AbstractNodeTest
	{
		[Test]
		public void RepeatActionUntilTerminateTest()
		{
			int max = 10;
			ActionTreeNode<int> action = CreateCounterAction(max);
			RepeatTreeNode repeat = new RepeatTreeNode(action);
			BehaviourTree btree = new BehaviourTree(repeat, gameObject);
			RunCounter(btree, max);
			Assert.AreEqual(max, action.Result);
			Assert.AreEqual(BehaviourTree.State.SUCCESS, action.GetState());
			Assert.AreEqual(BehaviourTree.State.SUCCESS, repeat.GetState());
		}

		[Test]
		public void RepeatBinaryTreeUntilTerminateTest()
		{
			int max = 10;
			ActionTreeNode<int> action = CreateCounterAction(max);
			BinaryTreeNode binary = new BinaryTreeNode(
				() => true,
				action,
				CreateImmediateAction(BehaviourTree.State.FAILURE)
			);
			RepeatTreeNode repeat = new RepeatTreeNode(binary);
			BehaviourTree btree = new BehaviourTree(repeat, gameObject);
			for (int i = 0; i < 2; i++) 
			{
				RunCounter(btree, max);
				Assert.AreEqual(BehaviourTree.State.SUCCESS, repeat.GetState());
				action.Result = 0;
			}
		}

		[Test]
		public void RepeatSelectorForAllChildrenTest() 
		{
			int max = 10;
			SelectorTreeNode selector = new SelectorTreeNode(new BehaviourTree.Node[] {
				CreateImmediateAction(BehaviourTree.State.FAILURE),
				CreateCounterAction(max)
			});
			RepeatTreeNode repeat = new RepeatTreeNode(selector);
			BehaviourTree btree = new BehaviourTree(repeat, gameObject);
			for (int i = 0; i < 1; i++) 
			{
				btree.Tick();
				Assert.AreEqual(BehaviourTree.State.RUNNING, selector.GetState());
				RunCounter(btree, max - 1);
				Assert.AreEqual(BehaviourTree.State.SUCCESS, selector.GetState());
			}
		}

		[Test]
		public void RepeatSequenceForAllChildrenTest() 
		{
			int max = 10;
			SequenceTreeNode sequence = new SequenceTreeNode(new BehaviourTree.Node[] {
				CreateCounterAction(max),
				CreateImmediateAction(BehaviourTree.State.SUCCESS)
			});
			RepeatTreeNode repeat = new RepeatTreeNode(sequence);
			BehaviourTree btree = new BehaviourTree(repeat, gameObject);
			for (int i = 0; i < 1; i++) 
			{
				RunCounter(btree, max);
				Assert.AreEqual(BehaviourTree.State.RUNNING, sequence.GetState());
				btree.Tick();
				Assert.AreEqual(BehaviourTree.State.SUCCESS, sequence.GetState());
			}
		}

		[Test]
		public void RepeatBinaryForAllChildrenTest()
		{
			int max = 10;
			bool flip = false;
			BinaryTreeNode binary = new BinaryTreeNode(
				() => (flip = !flip),
				CreateCounterAction(max),
				CreateImmediateAction(BehaviourTree.State.FAILURE)
			);
			RepeatTreeNode repeat = new RepeatTreeNode(binary);
			BehaviourTree btree = new BehaviourTree(repeat, gameObject);
			RunCounter(btree, max);
			Assert.AreEqual(BehaviourTree.State.SUCCESS, repeat.GetState());
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.FAILURE, repeat.GetState());
		}
	}
}
