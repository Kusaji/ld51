using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles triggering animations for enemies.
/// </summary>
public class EnemyAnimator : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    public EnemyController controller;
    public Animator anim;

    public enum currentState { idle, running, attacking};
    public currentState state;

    #endregion

    #region Unity Callbacks
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
    #endregion

    #region Methods
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
    #endregion
}
