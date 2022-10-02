using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;

/// <summary>
/// Base Class all deployable structures rely on.
/// </summary>
public class Structure : MonoBehaviour
{
    #region Variables
    [Header("Settings")]
    public int minimumPopulationToFunction = 0;
    public float holdTimeToStartAutoClick = 0.2f;
    public float effectivenessExponent = 0.75f;

    [Header("Health")]
    public bool isAlive;
    public float maxHealth;
    public float currentHealth;
    public float incomingDamageDefenseMod = 1f;

    [Header("Build Progress")]
    public bool buildingComplete;
    public float currentBuildProgress;
    public float maxBuildProgress;

    [Header("Stats")]
    public int designatedPopulation;
    public float cachedPopulationEffectiveness = 0f;
    public float populationAddedPerFUPGainPerSecond = 0.01f;
    public float populationAddedPerFUP = 0.1f;

    [Header("Prefabs")]
    public GameObject explosionPrefab;

    [Header("References")]
    public ActivePopulation myActivePopulation;
    public StructureHealthUI myStructureHealthUI;

    //private vars
    private float clickHeldTime = 0f;
    private float fakePopAdded = 0f;
    private bool leftClickHeld = false;
    private bool rightClickHeld = false;
    private bool constructionFrame = true;
    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;

        UpdatePopulation();

        if (myActivePopulation != null)
            myActivePopulation.SetPopulationCount(designatedPopulation, minimumPopulationToFunction);
        if (myStructureHealthUI != null)
            myStructureHealthUI.SetHealthCount(currentHealth, maxHealth);
    }
    private void FixedUpdate()
    {
        ManageHoldClickAddPopulation();

        if (constructionFrame && myStructureHealthUI != null)
            myStructureHealthUI.SetHealthCount(currentHealth, maxHealth);

        constructionFrame = false;
    }
    private void LateUpdate()
    {
        if (constructionFrame && myStructureHealthUI != null)
            myStructureHealthUI.SetHealthCount(currentHealth, maxHealth);
    }
    #endregion

    #region Custom Input Methods
    public virtual void OnClickDown()
    {
        //  Bastion doesn't take population, it always operates at full power.
        if (rightClickHeld == false && constructionFrame == false && myActivePopulation != null && PlayerStructures.instance.spawningTower == false)
        {
            leftClickHeld = true;
            fakePopAdded = 0f;
            clickHeldTime = 0f;
        }
    }

    public virtual void OnClickUp()
    {
        //  Bastion doesn't take population, it always operates at full power.
        if (rightClickHeld == false && leftClickHeld && myActivePopulation != null)
        {
            leftClickHeld = false;
            fakePopAdded = 0f;
            if (clickHeldTime < holdTimeToStartAutoClick && PlayerResources.Instance.population > 0)
                AddAPop();
            clickHeldTime = 0f;

        }
    }
    public virtual void OnRightClickDown()
    {
        //  Bastion doesn't take population, it always operates at full power.
        if (leftClickHeld == false && constructionFrame == false && myActivePopulation != null && PlayerStructures.instance.spawningTower == false)
        {
            rightClickHeld = true;
            fakePopAdded = 0f;
            clickHeldTime = 0f;
        }
    }

    public virtual void OnRightClickUp()
    {
        //  Bastion doesn't take population, it always operates at full power.
        if (leftClickHeld == false && rightClickHeld && myActivePopulation != null)
        {
            rightClickHeld = false;
            fakePopAdded = 0f;
            if (clickHeldTime < holdTimeToStartAutoClick && designatedPopulation > 0)
                RemoveAPop();
            clickHeldTime = 0f;

        }
    }
    #endregion

    #region Methods
    public void ManageHoldClickAddPopulation()
    {
        if (leftClickHeld && PlayerResources.Instance.population > 0)
        {
            clickHeldTime += Time.fixedDeltaTime;
            fakePopAdded += populationAddedPerFUP + (populationAddedPerFUPGainPerSecond * clickHeldTime);
            while (fakePopAdded >= 1 && PlayerResources.Instance.population > 0)
            {
                fakePopAdded--;
                AddAPop();
            }
        } else if (rightClickHeld && designatedPopulation > 0)
        {
            clickHeldTime += Time.fixedDeltaTime;
            fakePopAdded += (populationAddedPerFUP + (populationAddedPerFUPGainPerSecond * clickHeldTime)) / 2f;
            while (fakePopAdded >= 1 && designatedPopulation > 0)
            {
                fakePopAdded--;
                RemoveAPop();
            }
        }
    }

    private void AddAPop()
    {        
            designatedPopulation++;
            PlayerResources.Instance.SpendPopulation(1);
            UpdatePopulation();
    }

    private void RemoveAPop()
    {
        designatedPopulation--;
        PlayerResources.Instance.AddPopulation(1);
        UpdatePopulation();
    }

    /// <summary>
    /// Deals damage and determines if structure survived hit or not.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (myStructureHealthUI != null)
            myStructureHealthUI.SetHealthCount(currentHealth, maxHealth);

        if (currentHealth <= 0 && isAlive)
        {
            isAlive = false;
            
            //Remove from structurel ist
            if (PlayerStructures.instance.structures.Contains(gameObject))
            {
                PlayerStructures.instance.structures.Remove(gameObject);
                NavMeshBuilder.BuildNavMeshAsync();
            }

            //Explosion / Death prefab
            Instantiate(
                explosionPrefab,
                transform.position + new Vector3(0.0f, 0.25f, 0.0f),
                Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f)));

            Camera.main.GetComponent<CameraController>().ShakeCameraImpulse(Random.onUnitSphere, 10f);

            PlayerResources.Instance.TrackPopulationDestruction(designatedPopulation);

            GameObject.Destroy(gameObject);
        }
    }
    public virtual void UpdatePopulation()
    {
        if (designatedPopulation >= minimumPopulationToFunction)
            cachedPopulationEffectiveness = Mathf.Pow(designatedPopulation + 1, effectivenessExponent);
        else
            cachedPopulationEffectiveness = 0f;

        if (myActivePopulation != null)
        myActivePopulation.SetPopulationCount(designatedPopulation, minimumPopulationToFunction);
    }

    public void HealTower(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (myStructureHealthUI != null)
            myStructureHealthUI.SetHealthCount(currentHealth, maxHealth);
    }

    public void AddBuildProgress(float amount)
    {
        currentBuildProgress += amount;

        if (currentBuildProgress >= maxBuildProgress && !buildingComplete)
        {
            buildingComplete = true;
            OnBuildComplete();
        }
    }

    //janky quick and easy way to have towers "Activate" when building is complete.
    public void OnBuildComplete()
    {
        TowerTargeter targeter = GetComponent<TowerTargeter>();
        TowerAttacker attacker = GetComponent<TowerAttacker>();
        ResourceGenerator generator = GetComponent<ResourceGenerator>();
        RepairTower repair = GetComponent<RepairTower>();
        BuilderTower build = GetComponent<BuilderTower>();

        if (targeter != null)
        {
            targeter.enabled = true;
        }

        if (attacker != null)
        {
            attacker.enabled = true;
        }

        if (generator != null)
        {
            generator.enabled = true;
        }

        if (repair != null)
        {
            repair.enabled = true;
        }

        if (build != null)
        {
            build.enabled = true;
        }
    }
    #endregion
}
