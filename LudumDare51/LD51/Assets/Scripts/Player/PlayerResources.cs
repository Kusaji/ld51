using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;

    [Header("Player Status")]
    public bool isAlive;

    [Header("Resources")]
    public int population;
    public int units;
    public int magicRunes;


    private void Awake()
    {
        Instance = this;
        isAlive = true;
    }

    private void Start()
    {
    }

    public void AddPopulation(int amount)
    {
        population += amount;
    }

    public void SpendPopulation(int amount)
    {
        if (population >= amount)
        {
            population -= amount;
        }
        else
        {
            Debug.Log("Not enough population.");
        }
    }
}
