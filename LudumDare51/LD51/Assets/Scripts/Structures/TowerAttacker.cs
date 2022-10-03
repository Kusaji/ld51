using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles attack routines and dealing damage to enemies.
/// </summary>
public class TowerAttacker : MonoBehaviour
{
    #region Variables
    [Header("Options")]
    public bool instantAttack;
    
    [Header("Stats")]
    public float attackRange;
    public float attackDamage;
    public float attackDelay;
    public float areaOfEffect;
    public int multishot = 1;

    public float EffectiveAttackRange
    {
        get
        {
            return attackRange * structure.cachedPopulationRangeEffectiveness;
        }
    }

    [Header("References")]
    public TowerTargeter targeter;
    public Structure structure;
    public RangeIndicator rangeIndicator;
    
    [Header("Prefabs")]
    public GameObject attackProjectilePrefab;

    public bool meteorInvoker = false;

    //Private Vars
    private float tickingAttackCooldown;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        StartCoroutine(AttackRoutine());
        structure.OnUpdatePopulation.AddListener(SetRangeIndicator);
    }
    private void OnEnable()
    {
        SetRangeIndicator();
    }
    #endregion

    #region Methods
    public void DealDamage()
    {
        targeter.target.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
    }
    private void SetRangeIndicator()
    {
        rangeIndicator.SetRange(EffectiveAttackRange, RangeIndicator.IndicatorType.attacker);
    }
    #endregion

    #region Coroutines
    public IEnumerator AttackRoutine()
    {
        while (structure.isAlive)
        {
            if (targeter.target != null && structure.cachedPopulationEffectiveness > 0.001f)
            {
                int shotsFired = 0;
                if (multishot <= 1 && targeter.distanceToTarget <= EffectiveAttackRange)
                {
                    shotsFired++;
                    FireAtTarget(targeter.target);
                    
                } else if (multishot > 1)
                {
                    for (int i = 0; i < targeter.targets.Count; i++)
                    {
                        if (targeter.targets[i] != null)
                        {
                            if (Vector3.Distance(targeter.targets[i].transform.position, targeter.transform.position) <= EffectiveAttackRange)
                            {
                                shotsFired++;
                                FireAtTarget(targeter.targets[i]);
                            }
                        }
                    }
                }

                if (shotsFired > 0)
                {
                    structure.audioController.PlayOneShot(0, 0.60f);
                    tickingAttackCooldown = attackDelay;
                    while (tickingAttackCooldown >= 0f)
                    {
                        yield return new WaitForFixedUpdate();
                        tickingAttackCooldown -= Time.fixedDeltaTime * structure.cachedPopulationEffectiveness;
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private void FireAtTarget(GameObject target)
    {
        if (meteorInvoker)
        {
            var meatball = Instantiate(
                attackProjectilePrefab,
                targeter.projectileSpawnpoint.transform.position + Vector3.up * 15f,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f)));

            var meatcontroller = meatball.GetComponent<MeatballController>();
            meatcontroller.randomDirection = (target.transform.position - transform.position);
            meatcontroller.randomDirection.y = 0f;
            meatcontroller.randomDirection.Normalize();
            meatcontroller.directionSet = true;
        }
        else
        {

            var projectile = Instantiate(
                            attackProjectilePrefab,
                            targeter.projectileSpawnpoint.transform.position + Random.insideUnitSphere * targeter.randomSphereSpawnPoint,
                            Quaternion.identity);
            var projectileSettings = projectile.GetComponent<SingleTargetProjectile>();
            projectileSettings.target = target;
            projectileSettings.damage = attackDamage;
        }
    }
    #endregion
}
