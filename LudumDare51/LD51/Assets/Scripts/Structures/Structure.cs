using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;

public class Structure : MonoBehaviour
{

    [Header("Health")]
    public bool isAlive;
    public float maxHealth;
    public float currentHealth;
    public float incomingDamageDefenseMod = 1f;

    [Header("Build Progress")]
    public bool buildingComplete;
    public float buildingProgress;

    [Header("Stats")]
    public int designatedPopulation;

    [Header("Prefabs")]
    public GameObject explosionPrefab;

    public float effectivenessExponent = 0.75f;
    public float cachedPopulationEffectiveness = 0f;
    public int minimumPopulationToFunction = 0;

    private bool leftClickHeld = false;
    private bool rightClickHeld = false;
    private float clickHeldTime = 0f;
    public float holdTimeToStartAutoClick = 0.2f;
    public float populationAddedPerFUP = 0.1f;
    public float populationAddedPerFUPGainPerSecond = 0.01f;
    private float fakePopAdded = 0f;

    [Header("References")]
    public ActivePopulation myActivePopulation;
    public StructureHealthUI myStructureHealthUI;

    private bool constructionFrame = true;
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
        //float usedpop = designatedPopulation;
        //float finalEffectiveness = 0f;
        //int increments = 1;
        //while (usedpop >= effectivenessSoftCap)
        //{
        //    finalEffectiveness += 1f / increments;
        //    increments++;
        //    usedpop -= effectivenessSoftCap;
        //}
        //finalEffectiveness += Mathf.Lerp(0f, 1f / increments, SmoothFunc.SmoothStopVariable(usedpop / effectivenessSoftCap, 1.2f));
        if (designatedPopulation >= minimumPopulationToFunction)
            cachedPopulationEffectiveness = Mathf.Pow(designatedPopulation + 1, effectivenessExponent);
        else
            cachedPopulationEffectiveness = 0f;

        if (myActivePopulation != null)
        myActivePopulation.SetPopulationCount(designatedPopulation, minimumPopulationToFunction);
    }


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

    //Not sure what the best way for this would be.
    //Onclickdown start a coroutine for adding units?
    //Onclickup stop said coroutine?
    public virtual void OnClickStay()
    {

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

    //janky quick and easy way to have towers "Activate" when building is complete.
    public void OnBuildComplete()
    {
        TowerTargeter targeter = GetComponent<TowerTargeter>();
        TowerAttacker attacker = GetComponent<TowerAttacker>();
        ResourceGenerator generator = GetComponent<ResourceGenerator>();
        RepairTower repair = GetComponent<RepairTower>();

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
    }
}
