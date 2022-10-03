using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles Enemy Movement and Attacking AI.
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    public EnemyHealth health;
    public NavMeshAgent agent;
    public EnemyAnimator enemyAnimator;

    [Header("Set During Gameplay")]
    public GameObject target;
    public Structure targetStructure;

    [Header("Stats")]
    public float attackDamage;
    public float damagePerWave;
    public float defaultAttackRange;
    public float bastionAttackRange;
    public float attackDelay;
    public bool attackCooldown = false;
    public float startSpeed;
    public float speedPerWave;
    

    [Header("Runtime Stats")]
    public float currentAttackRange;
    public float distanceToTarget;
    public bool isAttacking;
    public float cachedSpeed;

    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        agent.Warp(transform.position);

        StartCoroutine(FindTargetRoutine());
        StartCoroutine(CalculateDistance());

        attackDamage = attackDamage + EnemyManager.Instance.wave * damagePerWave;
        cachedSpeed = startSpeed + EnemyManager.Instance.wave * speedPerWave;
        agent.speed = cachedSpeed;
        enemyAnimator.anim.speed *= cachedSpeed / 1.75f;
    }
    #endregion

    #region Methods
    //todo
    //Iterate over structure list and find closest with priority.
    public GameObject GetRandomTarget()
    {
        if (PlayerStructures.instance.structures.Count > 0)
        {
            return PlayerStructures.instance.structures[Random.Range(0, PlayerStructures.instance.structures.Count)];
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region Coroutines
    public IEnumerator FindTargetRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        while (health.isAlive)
        {
            if (PlayerStructures.instance.structures.Count > 0)
            {
                var closestTower = PlayerStructures.instance.structures[0];
                var closestTowerDistance = Mathf.Infinity;

                for (int i = 0; i < PlayerStructures.instance.structures.Count; i++)
                {
                    if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) < closestTowerDistance)
                    {
                        closestTowerDistance = Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position);
                        closestTower = PlayerStructures.instance.structures[i];
                    }
                }
                target = closestTower;
                targetStructure = closestTower.GetComponent<Structure>();
                agent.SetDestination(target.transform.position);
                distanceToTarget = closestTowerDistance;
                currentAttackRange = defaultAttackRange;


                //StartCoroutine(CalculateDistance());
            }
            else if (PlayerStructures.instance.bastion != null)
            {
                target = PlayerStructures.instance.bastion;

                if (target != null)
                {
                    targetStructure = PlayerStructures.instance.bastion.GetComponent<Structure>();
                    agent.SetDestination(target.transform.position);
                    currentAttackRange = bastionAttackRange;
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                enemyAnimator.anim.SetTrigger("Idle");
                distanceToTarget = 0;
            }
            yield return new WaitForSeconds(0.50f);
        }
    }

    public IEnumerator CalculateDistance()
    {
        while (health.isAlive)
        {
            if (target != null)
            {
                distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (distanceToTarget <= currentAttackRange && !isAttacking)
                {
                    StartCoroutine(AttackRoutine());
                    isAttacking = true;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public IEnumerator AttackRoutine()
    {
        while (health.isAlive != false)
        {
            if (distanceToTarget <= currentAttackRange && target != null)
            {
                targetStructure.DealDamage(attackDamage);
                enemyAnimator.AttackAnimation();
                attackCooldown = true;
                agent.speed = cachedSpeed * 0.05f;
            }
            else
            {
                isAttacking = false;
                yield break;
            }
            yield return new WaitForSeconds(attackDelay);
            attackCooldown = false;
            agent.speed = cachedSpeed;
        }
    }
    #endregion
}
