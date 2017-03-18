using BTree;
using Steer2D;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(EnemyActor))]
[RequireComponent(typeof(CollisionController))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Seek))]
[RequireComponent(typeof(Pathfinding2D))]
[AddComponentMenu("AI Behaviour Tree/Melee Attack")]
public class MeleeBehaviour : AbstractBTreeBehaviour
{
    public float meleeDelay = 0.1f;
    public float meleeTime = 0.1f;
    public float meleeRadius;
    public float attackRadius;
    
	private EnemyActor actor;
    private Seek seek;
	private List<Vector3> path;

    protected override void Start()
    {
        base.Start();
		actor = gameObject.GetComponent<EnemyActor>();
        seek = gameObject.GetComponent<Seek>();
        seek.enabled = false;
		path = new List<Vector3>();
    }

    private float setLookAtX(Vector3 target)
    {
        float lookX = target.x - transform.position.x;
        if (lookX < 0)
        {
            animationController.LookLeft();
        }
        else
        {
            animationController.LookRight();
        }
        return lookX;
    }

    public bool IsTargetInRange()
    {
		if (actor.GetTarget() != null && (actor.GetTarget().transform.position - transform.position).magnitude <= meleeRadius)
        {
			return true;
        }
        return false;
    }

    public bool IsTargetInAttackRange()
    {
		if (actor.GetTarget() != null && (actor.GetTarget().transform.position - transform.position).magnitude <= attackRadius)
        {
            return true;
        }
        return false;
    }

    public BehaviourTree.State Idle(BehaviourTreeNode<System.Object> node)
    {
        animationController.Idle();
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State Wake(BehaviourTreeNode<System.Object> node)
    {
        animationController.Wake();
        return BehaviourTree.State.SUCCESS;
    }

	public BehaviourTree.State GetPath(BehaviourTreeNode<System.Object> node) {
		BehaviourTree.State state = BehaviourTree.State.RUNNING;
		Pathfinder2D.Instance.FindPath(gameObject.transform.position, actor.GetTarget().transform.position, path => {
			if (path.Count > 0) 
			{
				this.path.Clear();
				this.path = path;
				Vector3 start = path.First();
				Vector3 end = path.Last();
				this.path[0] = new Vector3(start.x, start.y, start.z);
				this.path[path.Count - 1] = new Vector3(end.x, end.y, end.z);
			}
			state = BehaviourTree.State.SUCCESS;
		});
		return state;
	}

	public BehaviourTree.State MoveTowardsTarget(BehaviourTreeNode<System.Object> node)
    {
		GameObject target = actor.GetTarget();
		if (target == null || path.Count == 0 || Vector2.Distance(target.transform.position, path.Last()) > 0.4f)
        {
			path.Clear();
            seek.TargetPoint = transform.position;
            seek.enabled = false;
            animationController.Halt();
			return BehaviourTree.State.FAILURE;
        }
		seek.enabled = true;
		seek.TargetPoint = path[0];
		setLookAtX(path.Last());
		animationController.Move();
		if (Vector2.Distance(transform.position, path[0]) < 0.4f) {
			path.RemoveAt(0);
		}
		if (path.Count == 0 || IsTargetInAttackRange())
        {
            seek.TargetPoint = transform.position;
            seek.enabled = false;
            animationController.Halt();
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State PrepareAttack(BehaviourTreeNode<float> node)
    {
		if (actor.GetTarget() == null)
        {
            animationController.WithdrawMeleeAttack();
            return BehaviourTree.State.FAILURE;
        }
        animationController.PrepareMeleeAttack();
		setLookAtX(actor.GetTarget().transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > meleeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State Attack(BehaviourTreeNode<float> node)
    {
        animationController.MeleeAttack();
        node.Result += Time.deltaTime;
        if (node.Result > meleeTime)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node)
    {
        animationController.WithdrawMeleeAttack();
        return BehaviourTree.State.SUCCESS;
    }

    protected override BehaviourTree.Node Initialize()
    {
		return new SelectorTreeNode(new BehaviourTree.Node[] 
		{
			new BinaryTreeNode(
				IsTargetInRange,
				new SequenceTreeNode(new BehaviourTree.Node[] 
				{
					new ActionTreeNode<System.Object>(GetPath),
					new ActionTreeNode<System.Object>(Wake),
					new ActionTreeNode<System.Object>(MoveTowardsTarget),
					new BinaryTreeNode(
						IsTargetInAttackRange,
						new SequenceTreeNode(new BehaviourTree.Node[] 
						{
							new ActionTreeNode<float>(PrepareAttack),
							new ActionTreeNode<float>(Attack)
						}),
						new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
					)
				}),
				new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
			),
			new SequenceTreeNode(new BehaviourTree.Node[] 
			{
				new ActionTreeNode<System.Object>(WithdrawAttack),
				new ActionTreeNode<System.Object>(Idle),
				new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
			})
		});
    }
}
