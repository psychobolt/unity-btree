using UnityEditor;
using NUnit.Framework;

namespace BTree
{
	public class BinaryTreeNodeTest : AbstractNodeTest
	{
		[Test]
		public void IfTrueTest() 
		{
			BinaryTreeNode binary = new BinaryTreeNode(
				() => true,
				CreateImmediateAction(BehaviourTree.State.SUCCESS),
				CreateImmediateAction(BehaviourTree.State.FAILURE)
			);
			BehaviourTree btree = new BehaviourTree(binary, gameObject);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.SUCCESS, binary.GetState());
		}

		[Test]
		public void IfFalseTest() 
		{
			BinaryTreeNode binary = new BinaryTreeNode(
				() => false,
				CreateImmediateAction(BehaviourTree.State.SUCCESS),
				CreateImmediateAction(BehaviourTree.State.FAILURE)
			);
			BehaviourTree btree = new BehaviourTree(binary, gameObject);
			btree.Tick();
			Assert.AreEqual(BehaviourTree.State.FAILURE, binary.GetState());
		}
	}
}
