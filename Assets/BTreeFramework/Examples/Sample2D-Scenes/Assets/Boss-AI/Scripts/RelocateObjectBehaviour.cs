using System.Linq;
using BTree;
using UniRx;
using UnityEngine;

[AddComponentMenu("AI Behaviour Tree/Relocate Object")]
public class RelocateObjectBehaviour : AbstractBTreeBehaviour
{
    public GameObject target;

    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;
	public bool allowHidden;

    public Space space;

	private Vector3 position;
	private Vector3 size;

    public enum Space
    {
        SCREEN,
        WORLD
    }

	protected override void Start() {
		base.Start();
		size = gameObject.GetComponent<Renderer>().bounds.size;
	}

	private void GetNewPosition() {
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
				position = Camera.main.WorldToScreenPoint(position);
				position = Camera.main.ScreenToWorldPoint(new Vector3(
					freezeX ? position.x : Random.Range(0, Screen.width),
					freezeY ? position.y : Random.Range(0, Screen.height),
					freezeZ ? position.z : Camera.main.farClipPlane / 2
				));
				if (!allowHidden) {
					float width = size.x / 2;
					float height = size.y / 2;
					Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
					Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
					Rect cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
					position = new Vector3(
						Mathf.Clamp(position.x, cameraRect.xMin + width, cameraRect.xMax - width),
						Mathf.Clamp(position.y, cameraRect.yMin + height, cameraRect.yMax - height)
					);
				}
				break;
		}
	}

	public BehaviourTree.State IsValidPosition(BehaviourTreeNode<System.Object> node) {
		BehaviourTree.State state = node.State;
		Pathfinder2D.Instance.FindPath(gameObject.transform.position, position, path => {
			state = path.Count == 0 ? BehaviourTree.State.FAILURE : BehaviourTree.State.SUCCESS;
		});
		return state;
	}

    private void Relocate()
    {
		gameObject.transform.position = position;
    }

    protected override BehaviourTree.Node Initialize()
    {
		return new SequenceTreeNode(new BehaviourTree.Node[] {
			new ActionTreeNode<System.Object>(GetNewPosition),
			new ActionTreeNode<System.Object>(IsValidPosition),
			new ActionTreeNode<System.Object>(Relocate)
		});
    }
}