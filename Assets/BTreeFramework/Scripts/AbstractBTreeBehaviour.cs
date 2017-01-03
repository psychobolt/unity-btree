using BTree;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AbstractBTreeBehaviour : MonoBehaviour {

    public string parent;

    protected AnimationController animationController;

    protected virtual void Start()
    {
        animationController = new AnimationController(gameObject.GetComponent<Animator>());
    }

    public abstract BehaviourTree.Node GetBehaviourTree();

}
