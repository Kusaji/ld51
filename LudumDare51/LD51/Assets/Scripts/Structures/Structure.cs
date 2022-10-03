using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base Class all deployable structures rely on.
/// </summary>
public class Structure : MonoBehaviour
{
    #region Variables
    [Header("Structure Balance SO")]
    public StructureBalanceNumbersSO balanceNumbersSO;

    
    public int minimumPopulationToFunction
    {
        get
        {
            return balanceNumbersSO.minimumPopulationToFunction;
        }
    }
    [Header("Settings")]
    public float holdTimeToStartAutoClick = 0.2f;

    [Header("Health")]
    public bool isAlive;
    public float maxHealth
    {
        get
        {
            return balanceNumbersSO.maxHealth;
        }
    }
    public float currentHealth;
    public float incomingDamageDefenseMod
    {
        get
        {
            return balanceNumbersSO.incomingDamageDefenseMod;
        }
    }

    [Header("Build Progress")]
    public bool buildingComplete;
    public float currentBuildProgress;
    public float maxBuildProgress
    {
        get
        {
            return balanceNumbersSO.maxBuildProgress;
        }
    }

    [Header("Stats")]
    public int designatedPopulation;
    public float cachedPopulationEffectiveness = 0f;
    public float cachedPopulationRangeEffectiveness = 0f;
    public float populationAddedPerFUPGainPerSecond = 0.01f;
    public float populationAddedPerFUP = 0.1f;

    [Header("Prefabs")]
    public GameObject explosionPrefab;

    [Header("References")]
    public ActivePopulation myActivePopulation;
    public StructureHealthUI myStructureHealthUI;
    public AudioController audioController;

    //private vars
    private float clickHeldTime = 0f;
    private float fakePopAdded = 0f;
    private bool leftClickHeld = false;
    private bool rightClickHeld = false;
    private bool constructionFrame = true;

    public UnityEvent OnUpdatePopulation;
    #endregion

    private void Awake()
    {
        OnUpdatePopulation = new UnityEvent();
    }
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
        if (myStructureHealthUI != null)
            myStructureHealthUI.SetConstructionCount(currentBuildProgress, maxBuildProgress);        
    }
    private void FixedUpdate()
    {
        ManageHoldClickAddPopulation();

        if (constructionFrame && myStructureHealthUI != null)
            myStructureHealthUI.SetHealthCount(currentHealth, maxHealth);

        if (constructionFrame)
            UpdatePopulation();

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
                AddAPopFromPlayer();
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
            if (clickHeldTime < holdTimeToStartAutoClick && designatedPopulation > minimumPopulationToFunction)
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
                AddAPopFromPlayer();
            }
        } else if (rightClickHeld && designatedPopulation > minimumPopulationToFunction)
        {
            clickHeldTime += Time.fixedDeltaTime;
            fakePopAdded += (populationAddedPerFUP + (populationAddedPerFUPGainPerSecond * clickHeldTime)) / 2f;
            while (fakePopAdded >= 1 && designatedPopulation > minimumPopulationToFunction)
            {
                fakePopAdded--;
                RemoveAPop();
            }
        }
    }

    private void AddAPopFromPlayer()
    {        
            designatedPopulation++;
            PlayerResources.Instance.SpendPopulation(1);
            UpdatePopulation();
    }
    public void AddPopFromPlayer(int popnum)
    {
        designatedPopulation += popnum;
        PlayerResources.Instance.SpendPopulation(popnum);
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
                
                //TODO 
                //Rebuild navmesh
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
            cachedPopulationEffectiveness = Mathf.Pow(designatedPopulation + 1, balanceNumbersSO.effectivenessExponent);
        else
            cachedPopulationEffectiveness = 0f;

        cachedPopulationRangeEffectiveness = Mathf.Pow(designatedPopulation + 1, balanceNumbersSO.rangeEffectivenessExponent);

        if (myActivePopulation != null)
            myActivePopulation.SetPopulationCount(designatedPopulation, minimumPopulationToFunction);

        OnUpdatePopulation.Invoke();
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
            currentBuildProgress = maxBuildProgress;
        }
        if (myStructureHealthUI != null)
            myStructureHealthUI.SetConstructionCount(currentBuildProgress, maxBuildProgress);
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
