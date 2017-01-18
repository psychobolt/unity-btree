using System.Linq;

namespace BTree
{
    public class RepeatTreeNode : BehaviourTree.Node
    {
        public RepeatTreeNode(BehaviourTree.Node node) : base (new BehaviourTree.Node[] { node })
        {
        }

        protected override internal void Tick(BehaviourTree tree)
        {
            setExecutionCount(this, 0);
            base.Tick(tree);
        }

        protected override void Execute(BehaviourTree tree)
        {
			foreach (BehaviourTree.Node child in children[0].GetChildren())
            {
                setExecutionCount(child, 0);
            }
			children[0].Tick(tree);
        }

        public override BehaviourTree.Node[] GetNextChildren()
        {
            return children.Where(child => !child.IsTerminated()).ToArray();
        }

        protected override void OnExecute(BehaviourTree.Node child)
        {
            this.State = child.State;
        }
    }
}
