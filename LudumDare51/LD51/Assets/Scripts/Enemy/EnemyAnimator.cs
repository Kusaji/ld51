using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public EnemyController controller;
    public Animator anim;

    public List<Animation> attackAnimations;

    public bool isRunning;
    public bool isIdle;
    public bool isAttacking;

    //
    public enum currentState { idle, running, attacking};
    public currentState state;

    private void Start()
    {
        state = currentState.idle;
    }

    private void FixedUpdate()
    {
        if (controller.distanceToTarget > controller.currentAttackRange)
        {
            anim.SetTrigger("Run");
        }
        else 
        {
            anim.SetTrigger("Idle");
        }
    }

    public void AttackAnimation()
    {
        int randomAnimation = Random.Range(0, 3);

        switch (randomAnimation)
        {
            case 0:
                anim.SetTrigger("Attack1");
                break;
            case 1:
                anim.SetTrigger("Attack2");
                break;
            case 2:
                anim.SetTrigger("Attack3");
                break;
        }

    }
}
