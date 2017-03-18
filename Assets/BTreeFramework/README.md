# BTree Framework API

[About](https://github.com/psychobolt/unity-btree)

## Tree Nodes

### Composite
- SelectorTreeNode
- SequenceTreeNode
- RandomTreeNode
- BinaryTreeNode

### Decorators
- RepeatTreeNode

### Leaves
- ActionTreeNode

### States

Each node contains a state.

- INITIALIZED
- WAITING (waiting for children to be finished)
- SUCCESS
- FAILURE
- RUNNING
- TERMINATED (if a node is interrupt)

## BTree Behaviors

BTree behaviors extends MonoBehaviour for providing Inspector support. BTree behaviors consist of a root Behavior Tree.

The behavior tree can be added to Game Objects using Unity's menu: 
> Components > AI Behaviour Tree

The behavior tree can be created by extending AbstractBTreeBehaviour or using Unity's menu:
> Assets > Create > Behaviour Tree

The behavior tree can have one or more specified parents. The "parents" property can take an array of Group Names.

## AbstractAnimatorController

When using BTree behaviors, an implementation of AbstractAnimatorController should be added as a component of the Game Object. This component interfaces with Mecanim Animation Controller.

## BTree Groups

Groups provide linking of different Behavior Trees to form a hierarchy. A group's children are ordered left to right which is respective to Unity's Inspector top to bottom ordering. Groups are analogous to Composite and Decorator nodes.

### Composite
- BTreeSelectorGroup
- BTreeSequenceGroup

### Decorators
- BTreeRandomizeGroup

### Side-effect Groups

Sometimes we want to perform an side effect when a Tree is executed. The following are groups that provide side-effects during a tree tick.
- [BTreeInterruptGroup](Scripts/BTreeInterruptGroup.md)

## Examples

[2D Scenes](Examples/Sample2D-Scenes/README.md)

### License

Examples may use placeholder assets and are not for commerical use. All original libraries and assets belong to their respective owners and creators. 