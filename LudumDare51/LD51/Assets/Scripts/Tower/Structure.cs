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


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
