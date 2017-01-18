using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSpriteScale : StateMachineBehaviour {

    public Vector3 scale = Vector3.one;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.transform.localScale = scale;
    }
}
