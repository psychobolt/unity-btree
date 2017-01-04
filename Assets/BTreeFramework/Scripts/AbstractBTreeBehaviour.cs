using BTree;
using UnityEngine;

public abstract class AbstractBTreeBehaviour : MonoBehaviour {

    public string parent;

    protected AbstractAnimationController animationController;

    protected virtual void Start()
    {
        animationController = gameObject.GetComponent<AbstractAnimationController>();
    }

    public abstract BehaviourTree.Node GetBehaviourTree();

}
