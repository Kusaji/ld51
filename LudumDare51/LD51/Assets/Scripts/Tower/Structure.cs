using System.Collections;
using System.Collections.Generic;
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
    protected float cachedPopulationEffectiveness = 0f;
    public int minimumPopulationToFunction = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && isAlive)
        {
            isAlive = false;
            
            //Remove from structurel ist
            if (PlayerStructures.instance.structures.Contains(gameObject))
            {
                PlayerStructures.instance.structures.Remove(gameObject);
            }

            //Explosion / Death prefab
            Instantiate(
                explosionPrefab,
                transform.position + new Vector3(0.0f, 0.25f, 0.0f),
                Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f)));

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

    }
}
