using BTree;
using UnityEngine;

[RequireComponent(typeof(BTreeTickBehaviour))]
public abstract class AbstractBTreeBehaviour : MonoBehaviour {

    public abstract BehaviourTree.Node GetBehaviourTree();

}
