using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBoolProperty : StateMachineBehaviour
{
    public string property;
    public bool value;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(property, value);
    }
}
