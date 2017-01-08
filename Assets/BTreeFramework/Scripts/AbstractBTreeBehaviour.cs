using BTree;
using UnityEngine;
using System;

public abstract class AbstractBTreeBehaviour : MonoBehaviour {
	
	public string[] parents = new string[] {};

    protected AbstractAnimationController animationController;

	private BehaviourTree.Node rootNode;

	public AbstractBTreeBehaviour GetRoot() {
		foreach (AbstractBTreeGroup btreeGroup in gameObject.GetComponents<AbstractBTreeGroup>()) {
			if (Array.Exists(parents, parent => parent == btreeGroup.groupName)) {
				AbstractBTreeBehaviour parent = btreeGroup.GetRoot();
				return parent ?? btreeGroup;
			}
		}
		return this;
	}

	public BehaviourTree.Node GetBehaviourTree() {
		return rootNode ?? (rootNode = Initialize());
	}

    protected virtual void Start()
    {
        animationController = gameObject.GetComponent<AbstractAnimationController>();
		if (!gameObject.GetComponent<BTreeTickBehaviour>()) {
			gameObject.AddComponent<BTreeTickBehaviour>();
		}
    }

    protected abstract BehaviourTree.Node Initialize();

}
