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

    [Header("Stats")]
    public float buildingProgress;
    public int designatedPopulation;

    [Header("Prefabs")]
    public GameObject explosionPrefab;

    public float effectivenessExponent = 0.75f;
    public float cachedPopulationEffectiveness = 0f;
    public int minimumPopulationToFunction = 0;

    private bool clickHeld = false;
    private float clickHeldTime = 0f;
    public float holdTimeToStartAutoClick = 0.2f;
    public float populationAddedPerFUP = 0.1f;
    public float populationAddedPerFUPGainPerSecond = 0.01f;
    private float fakePopAdded = 0f;
    public ActivePopulation myActivePopulation;
    private bool constructionFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;

        UpdatePopulation();

        if (myActivePopulation != null)
            myActivePopulation.SetPopulationCount(designatedPopulation, minimumPopulationToFunction);
    }
    private void FixedUpdate()
    {
        ManageHoldClickAddPopulation();
        constructionFrame = false;
    }
    public void ManageHoldClickAddPopulation()
    {
        if (clickHeld && PlayerResources.Instance.population > 0)
        {
            clickHeldTime += Time.fixedDeltaTime;
            fakePopAdded += populationAddedPerFUP + (populationAddedPerFUPGainPerSecond * clickHeldTime);
            while (fakePopAdded >= 1 && PlayerResources.Instance.population > 0)
            {
                fakePopAdded--;
                AddAPop();
            }
        }
    }
    private void AddAPop()
    {
        
            designatedPopulation++;
            PlayerResources.Instance.SpendPopulation(1);
            UpdatePopulation();
        
    }
    public virtual void DealDamage(float damage)
    {
        currentHealth -= damage;

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
        Debug.Log($"{gameObject.name} OnClickDown called");
        //  Bastion doesn't take population, it always operates at full power.
        if (constructionFrame == false && myActivePopulation != null && PlayerStructures.instance.spawningTower == false)
        {
            clickHeld = true;
            fakePopAdded = 0f;
            clickHeldTime = 0f;
        }
    }

    public virtual void OnClickUp()
    {
        Debug.Log($"{gameObject.name} OnClickUp called");
        //  Bastion doesn't take population, it always operates at full power.
        if (clickHeld && myActivePopulation != null)
        {
            clickHeld = false;
            fakePopAdded = 0f;
            if (clickHeldTime < holdTimeToStartAutoClick && PlayerResources.Instance.population > 0)
                AddAPop();
            clickHeldTime = 0f;
            
        }
    }

    //Not sure what the best way for this would be.
    //Onclickdown start a coroutine for adding units?
    //Onclickup stop said coroutine?
    public virtual void OnClickStay()
    {

    }
}
