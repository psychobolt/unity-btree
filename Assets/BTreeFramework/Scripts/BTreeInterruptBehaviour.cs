using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using System;
using UniRx;
using UniRx.Triggers;

public class BTreeInterruptBehaviour : MonoBehaviour {

	public string groupName;
	public string[] interuptRootGroups = new string[] {};
	public BehaviourTree.State interruptOnState;

	private HashSet<string> interruptGroups;
	private BTreeTickBehaviour tickBehaviour;

	void Start() {
		interruptGroups = new HashSet<string>();
		tickBehaviour = gameObject.GetComponent<BTreeTickBehaviour>();
		tickBehaviour.GroupsAsObservable().Subscribe(groups => {
			foreach (AbstractBTreeBehaviour behaviour in gameObject.GetComponents<AbstractBTreeBehaviour>())
			{
				if (!behaviour.enabled)
				{
					continue;
				}
				if (Array.Exists(behaviour.parents, parent => parent == groupName))
				{
					AbstractBTreeBehaviour root = behaviour.GetRoot();
					root.GetBehaviourTree().OnExecute().Subscribe(state => {
						if (state == interruptOnState) {
							foreach (string groupName in interruptGroups) {
								groups[groupName].Lock();
							}
						} 
						else {
							foreach (string groupName in interruptGroups) {
								groups[groupName].Unlock();
							}
						}
					});
				}
				if (behaviour is AbstractBTreeGroup) {
					AbstractBTreeGroup group = (AbstractBTreeGroup) behaviour;
					if (Array.Exists(interuptRootGroups, root => root == group.groupName)) {
						group = (AbstractBTreeGroup) behaviour.GetRoot();
						interruptGroups.Add(group.groupName);
					}
				}
			}
		});
	}
}
