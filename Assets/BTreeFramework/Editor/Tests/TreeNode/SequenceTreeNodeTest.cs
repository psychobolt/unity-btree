using NUnit.Framework;
using UnityEngine;

namespace BTree
{
	public class SequenceTreeNodeTest : AbstractNodeTest
	{
		[Test]
		public void ExecuteNoChildrenTest() {
			SequenceTreeNode sequence = new SequenceTreeNode(new BehaviourTree.Node[] {});
			BehaviourTree btree = new BehaviourTree(sequence, gameObject);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.WAITING, sequence.GetState());
		}

		[Test]
		public void ExecuteAllTest()
		{
			int max = 10;
			SequenceTreeNode sequence = new SequenceTreeNode(new BehaviourTree.Node[] {
				CreateCounterAction(max),
				CreateImmediateAction(BehaviourTree.State.SUCCESS)
			});
			BehaviourTree btree = new BehaviourTree(sequence, gameObject);
			RunCounter(btree, max);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.SUCCESS, sequence.GetState());
		}

		[Test]
		public void ExecuteSomeTest()
		{
			int max = 10;
			SequenceTreeNode sequence = new SequenceTreeNode(new BehaviourTree.Node[] {
				CreateCounterAction(max),
				CreateImmediateAction(BehaviourTree.State.SUCCESS),
				CreateImmediateAction(BehaviourTree.State.SUCCESS),
				CreateImmediateAction(BehaviourTree.State.FAILURE),
				CreateImmediateAction(BehaviourTree.State.SUCCESS)
			});
			BehaviourTree btree = new BehaviourTree(sequence, gameObject);
			RunCounter(btree, max);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.WAITING, sequence.GetState());
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.FAILURE, sequence.GetState());
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.FAILURE, sequence.GetState());
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.FAILURE, sequence.GetState());
		}
	}
}
