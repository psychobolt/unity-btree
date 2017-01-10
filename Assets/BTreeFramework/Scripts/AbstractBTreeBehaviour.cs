using System;
using UnityEngine;

namespace BTree
{
    public abstract class AbstractBTreeBehaviour : MonoBehaviour
    {

        public string[] parents = new string[] { };

        protected AbstractAnimationController animationController;

        private BehaviourTree.Node rootNode;

        public AbstractBTreeBehaviour GetRoot()
        {
            foreach (AbstractBTreeGroup btreeGroup in gameObject.GetComponents<AbstractBTreeGroup>())
            {
                if (Array.Exists(parents, parent => parent == btreeGroup.groupName))
                {
                    AbstractBTreeBehaviour parent = btreeGroup.GetRoot();
                    return parent ?? btreeGroup;
                }
            }
            return this;
        }

        public BehaviourTree.Node GetBehaviourTree()
        {
            return rootNode ?? (rootNode = Initialize());
        }

        protected virtual void Start()
        {
            BTreeTickBehaviour tickBehaviour = gameObject.GetComponent<BTreeTickBehaviour>();
            if (!tickBehaviour)
            {
                tickBehaviour = gameObject.AddComponent<BTreeTickBehaviour>();
            }
            animationController = gameObject.GetComponent<AbstractAnimationController>();
        }

        protected abstract BehaviourTree.Node Initialize();

    }
}
