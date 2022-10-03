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
                if (targeter.distanceToTarget <= EffectiveAttackRange)
                {
                    if (instantAttack)
                    {
                        targeter.target.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                        Debug.DrawLine(targeter.projectileSpawnpoint.transform.position, targeter.target.transform.position, Color.red, 0.35f);
                    }
                    else if (!instantAttack)
                    {
                        var projectile = Instantiate(
                            attackProjectilePrefab,
                            targeter.projectileSpawnpoint.transform.position + Random.insideUnitSphere * targeter.randomSphereSpawnPoint,
                            Quaternion.identity);
                        var projectileSettings = projectile.GetComponent<SingleTargetProjectile>();
                        projectileSettings.target = targeter.target;
                        projectileSettings.damage = attackDamage;
                        structure.audioController.PlayOneShot(0, 0.60f);
                    }

                    tickingAttackCooldown = attackDelay;
                    while (tickingAttackCooldown >= 0f)
                    {
                        yield return new WaitForFixedUpdate();
                        tickingAttackCooldown -= Time.fixedDeltaTime * structure.cachedPopulationEffectiveness;
                    }
                    //yield return new WaitForSeconds(attackDelay / structure.cachedPopulationEffectiveness);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
