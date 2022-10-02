using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public Structure myStructure;
    public float timeToGenerateResource = 10f;
    public float generateTimer;
    public ParticleSystem generateEffect;
    private void Awake()
    {
        generateTimer = timeToGenerateResource;
    }
    private void FixedUpdate()
    {

        generateTimer -= Time.fixedDeltaTime * myStructure.cachedPopulationEffectiveness;
        int generates = 0;
        while (generateTimer <= 0f)
        {
            PlayerResources.Instance.AddPopulation(1);
            PlayerResources.Instance.TrackPopulationGeneration(1);
            generateTimer += timeToGenerateResource;
            generates++;
        }
        if (generates > 0)
            generateEffect.Play();
    }
}
