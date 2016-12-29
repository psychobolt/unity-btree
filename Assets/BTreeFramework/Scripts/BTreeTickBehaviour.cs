using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;

public class BTreeTickBehaviour : MonoBehaviour {

	private BehaviourTree btree;

	// Use this for initialization
	void Start () {
		btree = new BehaviourTree(gameObject.GetComponent<Actor>().GetBehaviourTree(), gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        btree.Tick();
	}
}
