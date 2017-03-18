using BTree;
using Sample2D;
using UnityEngine;

[RequireComponent(typeof(EnemyActor))]
[RequireComponent(typeof(CollisionController))]
[RequireComponent(typeof(CircleCollider2D))]
[AddComponentMenu("AI Behaviour Tree/Range Attack")]
public class RangeBehaviour : AbstractBTreeBehaviour
{
    public float rangeRadius;
    public float rangeDelay = 0.1f;
    public float rangeTime = 0.1f;
    public string projectileBehaviorName;
    private AbstractProjectileBehaviour projectileBehavior;
    
	private EnemyActor actor;

    protected override void Start ()
    {
        base.Start();
		actor = gameObject.GetComponent<EnemyActor>();
        InitExternalBehaviors();
    }

    private void InitExternalBehaviors()
    {
        foreach (Component component in gameObject.GetComponents(typeof(AbstractProjectileBehaviour)))
        {
            AbstractProjectileBehaviour behavior = (AbstractProjectileBehaviour)component;
            if (component.GetType().Name == projectileBehaviorName)
            {
                projectileBehavior = behavior;
                projectileBehavior.enabled = false;
            }
        }
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
		return actor.GetTarget() != null;
    }

    public bool IsTargetInRangeRadius()
    {
		if (actor.GetTarget() != null && (actor.GetTarget().transform.position - transform.position).magnitude > rangeRadius)
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

    public BehaviourTree.State LookAtTarget(BehaviourTreeNode<System.Object> node)
    {
		if (actor.GetTarget() == null)
        {
            return BehaviourTree.State.SUCCESS;
        }
		setLookAtX(actor.GetTarget().transform.position);
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State PrepareAttack(BehaviourTreeNode<float> node)
    {
		if (actor.GetTarget() == null)
        {
            return BehaviourTree.State.FAILURE;
        }
        animationController.PrepareRangeAttack();
		setLookAtX(actor.GetTarget().transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > rangeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State Attack(BehaviourTreeNode<float> node)
    {
        animationController.RangeAttack();
        if (projectileBehavior && node.Result == 0f)
        {
            projectileBehavior.Fire();
        }
        node.Result += Time.deltaTime;
        if (node.Result > rangeTime)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node)
    {
        animationController.WithdrawRangeAttack();
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
					new ActionTreeNode<System.Object>(LookAtTarget),
					new BinaryTreeNode(
						IsTargetInRangeRadius,
						new SequenceTreeNode(new BehaviourTree.Node[]
						{
							new ActionTreeNode<System.Object>(Wake),
							new ActionTreeNode<float>(PrepareAttack),
							new ActionTreeNode<float>(Attack)
						}),
						new ActionTreeNode<System.Object>(WithdrawAttack)	
					)
				}),
				new ActionTreeNode<System.Object>(WithdrawAttack)
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
