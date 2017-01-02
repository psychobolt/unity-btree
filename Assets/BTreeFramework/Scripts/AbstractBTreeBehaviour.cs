using BTree;
using UnityEngine;

public abstract class AbstractBTreeBehaviour : MonoBehaviour {

    public string parent;

    public abstract BehaviourTree.Node GetBehaviourTree();

}
