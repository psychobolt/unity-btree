using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;

public class BTreeTickBehaviour : MonoBehaviour {

	private BehaviourTree btree;
    
	void Start () {
        List<BehaviourTree.Node> behaviours = new List<BehaviourTree.Node>();
        foreach(Component component in gameObject.GetComponents(typeof(AbstractBTreeBehaviour)))
        {
            AbstractBTreeBehaviour behaviour = (AbstractBTreeBehaviour)component;
            behaviours.Add(behaviour.GetBehaviourTree());
        }
        btree = new BehaviourTree(new RepeatTreeNode(new SelectorTreeNode(behaviours.ToArray())), gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        btree.Tick();
	}
}
