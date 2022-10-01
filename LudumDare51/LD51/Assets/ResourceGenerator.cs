using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : Structure
{
    public float timeToGenerateResource = 10f;
    public float generateTimer;
    public ParticleSystem generateEffect;
    private void Awake()
    {
        generateTimer = timeToGenerateResource;
    }
    private void FixedUpdate()
    {
        HoldClickAddPopulation();

        generateTimer -= Time.fixedDeltaTime * cachedPopulationEffectiveness;
        int generates = 0;
        while (generateTimer <= 0f)
        {
            PlayerResources.Instance.AddPopulation(1);
            generateTimer += timeToGenerateResource;
            generates++;
        }
        if (generates > 0)
            generateEffect.Play();
    }
}
