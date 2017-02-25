using System.Linq;
using BTree;
using UniRx;
using UnityEngine;

[AddComponentMenu("AI Behaviour Tree/Relocate Object")]
public class RelocateObjectBehaviour : AbstractBTreeBehaviour
{
    public GameObject target;
    public GameObject[] areas;

    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;
	public bool allowHidden;
    public bool failIfInvalid;

	private Vector3 position;
	private Vector3 size;
    private bool valid;

	protected override void Start() {
		base.Start();
		size = gameObject.GetComponent<Renderer>().bounds.size;
	}

	private void GetNewPosition() {
        System.Random random = new System.Random();
        int index = random.Next(areas.Length);
        GameObject area = areas[index];
        Camera camera = area.GetComponent<Camera>();
        SphereCollider sphereCollider = area.GetComponent<SphereCollider>();
        CircleCollider2D circleCollider = area.GetComponent<CircleCollider2D>();
        if (camera)
        {
            position = camera.WorldToScreenPoint(position);
            position = camera.ScreenToWorldPoint(new Vector3(
                freezeX ? position.x : Random.Range(0, Screen.width),
                freezeY ? position.y : Random.Range(0, Screen.height),
                freezeZ ? position.z : Camera.main.farClipPlane / 2
            ));
            if (!allowHidden)
            {
                float width = size.x / 2;
                float height = size.y / 2;
                Vector3 bottomLeft = camera.ScreenToWorldPoint(Vector3.zero);
                Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight));
                Rect cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
                position = new Vector3(
                    Mathf.Clamp(position.x, cameraRect.xMin + width, cameraRect.xMax - width),
                    Mathf.Clamp(position.y, cameraRect.yMin + height, cameraRect.yMax - height)
                );
            }
        }
        else if (sphereCollider)
        {
            Vector3 newPosition = area.transform.TransformPoint(sphereCollider.center) + sphereCollider.radius * Random.insideUnitSphere;
            position = new Vector3(
                freezeX ? position.x : newPosition.x,
                freezeY ? position.y : newPosition.y,
                freezeZ ? position.z : newPosition.z
            );
        }
        else if (circleCollider)
        {
            Vector2 localPosition = circleCollider.radius* Random.insideUnitCircle;
            Vector3 newPosition = area.transform.TransformPoint(circleCollider.offset) + new Vector3(localPosition.x, localPosition.y, 0);
            position = new Vector3(
                freezeX ? position.x : newPosition.x,
                freezeY ? position.y : newPosition.y,
                freezeZ ? position.z : newPosition.z
            );
        }
        else {
            position = target.transform.position;
            position = new Vector3(
                freezeX ? position.x : Random.Range(float.MinValue, float.MaxValue),
                freezeY ? position.y : Random.Range(float.MinValue, float.MaxValue),
                freezeZ ? position.z : Random.Range(float.MinValue, float.MaxValue)
            );
        }
	}

	public BehaviourTree.State IsValidPosition(BehaviourTreeNode<System.Object> node) {
		BehaviourTree.State state = node.State;
        try
        {
            Pathfinder2D.Instance.FindPath(gameObject.transform.position, position, path =>
            {
                valid = path.Count > 0;
                state = !valid && failIfInvalid ? BehaviourTree.State.FAILURE : BehaviourTree.State.SUCCESS;
            });
        }
        catch (System.IndexOutOfRangeException)
        {
            state = failIfInvalid ? BehaviourTree.State.FAILURE : BehaviourTree.State.SUCCESS;
        }
		return state;
	}

    private void Relocate()
    {
        if (valid) {
            gameObject.transform.position = position;
        }
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