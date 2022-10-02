using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles logic for repair tower.
/// </summary>
public class RepairTower : MonoBehaviour
{
    #region Variables
    [Header("Tower Lists")]
    public List<Structure> towersInRange;
    public List<Structure> damagedTowers;

    [Header("Can heal self?")]
    public bool canHealSelf;

    [Header("Stats - Do Set")]
    public float towerHealRange;
    public float towerHealAmount;
    public float towerHealDelay;

    [Header("Stats - Set Via Game")]
    public int activeHealTargets;
    public float tickingHealCooldown;

    [Header("Prefabs")]
    public GameObject repairProjectile;

    [Header("Prefab Origins")]
    public GameObject projectileOrigin;

    private Structure myStructure;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        myStructure = GetComponent<Structure>();
        GetTowersInRange();
        StartCoroutine(HealRoutine());
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        PlayerStructures.TowerUpdate += GetTowersInRange;
    }

    private void OnDisable()
    {
        PlayerStructures.TowerUpdate -= GetTowersInRange;
    }
    #endregion

    #region Methods

    public void GetTowersInRange()
    {
        towersInRange.Clear();

        if (PlayerStructures.instance.structures.Count > 0)
        {
            for (int i = 0; i < PlayerStructures.instance.structures.Count; i++)
            {
                if (!canHealSelf)
                {
                    if (PlayerStructures.instance.structures[i].gameObject != gameObject) //Check to make sure we don't add ourself to list.
                    {
                        if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) <= towerHealRange)
                        {
                            towersInRange.Add(PlayerStructures.instance.structures[i].GetComponent<Structure>());
                        }
                    }
                }
                else if (canHealSelf) //Bypass check and have ourself on list to be healed.
                {
                    if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) <= towerHealRange)
                    {
                        towersInRange.Add(PlayerStructures.instance.structures[i].GetComponent<Structure>());
                    }
                }
            }
        }
    }

    public void CheckForDamagedTowers()
    {
        damagedTowers.Clear();

        for (int i = 0; i < towersInRange.Count; i++)
        {
            if (towersInRange[i].currentHealth < towersInRange[i].maxHealth)
            {
                damagedTowers.Add(towersInRange[i]);
            }
        }

        activeHealTargets = damagedTowers.Count;
    }


    public void HealStructures()
    {
        float healAmount = towerHealAmount / activeHealTargets;

        for (int i = 0; i < damagedTowers.Count; i++)
        {
            if (damagedTowers[i] != null) //In the off chance it was destroyed before the heal goes off.
            {
                damagedTowers[i].HealTower(healAmount);
                
                //spawn our heal effect
                var healProjectile = Instantiate(
                    repairProjectile,
                    projectileOrigin.transform.position,
                    Quaternion.identity
                    );

                healProjectile.GetComponent<RepairTowerProjectile>().target = damagedTowers[i].gameObject;
            }
        }
    }
    #endregion

    #region Coroutines
    public IEnumerator HealRoutine()
    {
        while (myStructure.isAlive)
        {
            if (towersInRange.Count > 0)
            {
                CheckForDamagedTowers();

                if (damagedTowers.Count > 0)
                {
                    HealStructures();
                }
            }

            tickingHealCooldown = towerHealDelay;

            while (tickingHealCooldown >= 0f)
            {
                yield return new WaitForFixedUpdate();
                tickingHealCooldown -= Time.fixedDeltaTime * myStructure.cachedPopulationEffectiveness;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
