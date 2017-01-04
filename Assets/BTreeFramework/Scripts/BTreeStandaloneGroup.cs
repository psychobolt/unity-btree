using BTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTreeStandaloneGroup : MonoBehaviour {
    
    private List<BehaviourTree> btrees;

    void Start () {
        btrees = new List<BehaviourTree>();
        foreach (Component component in gameObject.GetComponents(typeof(AbstractBTreeBehaviour)))
        {
            AbstractBTreeBehaviour behaviour = (AbstractBTreeBehaviour)component;
            if (behaviour.enabled && string.IsNullOrEmpty(behaviour.parent))
            {
                btrees.Add(new BehaviourTree(behaviour.GetBehaviourTree(), gameObject));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(BehaviourTree btree in btrees) 
        {
            btree.Tick();
        }
    }
}
