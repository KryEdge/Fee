using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMilestonePanel : MonoBehaviour
{
    private Animator animator;
    private bool animationSwitch;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchAnimation()
    {
        animationSwitch = !animationSwitch;

        if(animationSwitch)
        {
            animator.SetBool("isHiding", true);
            animator.SetBool("isShowing", false);
        }
        else
        {
            animator.SetBool("isHiding", false);
            animator.SetBool("isShowing", true);
        }
    }
}
