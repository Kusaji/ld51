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
    //public Animator anim;
    public AnimatedMesh anim;

    public enum currentState { idle, running, attacking};
    public currentState state;

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        state = currentState.idle;
        //anim.speed = Random.Range(0.95f, 1.05f);
    }

    private void FixedUpdate()
    {
        if (controller.distanceToTarget > controller.currentAttackRange)
        {
            anim.Play("BasicEnemy_WalkForward");
        }
        else 
        {
            anim.Play("BasicEnemy_idle");
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
                anim.Play("BasicEnemy_Attack");
                break;
            case 1:
                anim.Play("BasicEnemy_Attack2");
                break;
            case 2:
                anim.Play("BasicEnemy_Attack3");
                break;
        }
    }
    #endregion
}
