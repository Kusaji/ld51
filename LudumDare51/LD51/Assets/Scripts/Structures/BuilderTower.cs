using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior for the builder tower.
/// </summary>
public class BuilderTower : MonoBehaviour
{
    #region Variables
    [Header("Lists")]
    public List<Structure> towersInRange;
    public List<Structure> incompleteTowers;

    [Header("Active Targets")]
    public int activeBuildTargets;

    [Header("Stats")]
    public float buildRange;
    public float buildPower;
    public float tickingBuildCooldown;
    public float towerBuildDelay;

    [Header("Components")]
    public Structure myStructure;
    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        GetTowersInRange();
        StartCoroutine(BuildRoutine());
    }
    #endregion

    #region Events and Delegates
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
                if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) <= buildRange)
                {
                    towersInRange.Add(PlayerStructures.instance.structures[i].GetComponent<Structure>());
                }
            }
        }
    }

    public void CheckForIncompleteTowers()
    {
        incompleteTowers.Clear();

        for (int i = 0; i < towersInRange.Count; i++)
        {
            if (towersInRange[i].currentBuildProgress < towersInRange[i].maxBuildProgress)
            {
                incompleteTowers.Add(towersInRange[i]);
            }
        }

        activeBuildTargets = incompleteTowers.Count;
    }

    public void BuildStructures()
    {
        float buildAmount = buildPower / activeBuildTargets;

        for (int i = 0; i < incompleteTowers.Count; i++)
        {
            if (incompleteTowers[i] != null) //In the off chance it was destroyed before the heal goes off.
            {
                incompleteTowers[i].AddBuildProgress(buildAmount);
            }
        }
    }
    #endregion

    #region Coroutines
    public IEnumerator BuildRoutine()
    {
        while (myStructure.isAlive)
        {
            if (towersInRange.Count > 0)
            {
                CheckForIncompleteTowers();

                if (incompleteTowers.Count > 0)
                {
                    BuildStructures();
                }
            }

            tickingBuildCooldown = towerBuildDelay;

            while (tickingBuildCooldown >= 0f)
            {
                yield return new WaitForFixedUpdate();
                tickingBuildCooldown -= Time.fixedDeltaTime * myStructure.cachedPopulationEffectiveness;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
