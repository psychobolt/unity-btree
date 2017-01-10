using BTree;
using UnityEngine;

[AddComponentMenu("AI Behaviour Tree/Relocate Object")]
public class RelocateObjectBehaviour : AbstractBTreeBehaviour
{
    public GameObject target;

    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;

    public Space space;

    public enum Space
    {
        SCREEN,
        WORLD
    }

    private void Relocate()
    {
        Vector3 position;
        switch (space)
        {
            case Space.WORLD:
                position = target.transform.position;
                position = new Vector3(
                    freezeX ? position.x : Random.Range(float.MinValue, float.MaxValue),
                    freezeY ? position.y : Random.Range(float.MinValue, float.MaxValue),
                    freezeZ ? position.z : Random.Range(float.MinValue, float.MaxValue)
                );
                break;
            default:
                position = Camera.main.WorldToScreenPoint(target.transform.position);
                position = Camera.main.ScreenToWorldPoint(new Vector3(
                    freezeX ? position.x : Random.Range(0, Screen.width),
                    freezeY ? position.y : Random.Range(0, Screen.height),
                    freezeZ ? position.z : Camera.main.farClipPlane / 2
                ));
                break;
        }
        gameObject.transform.position = position;
    }

    protected override BehaviourTree.Node Initialize()
    {
        return new ActionTreeNode<System.Object>(Relocate);
    }
}