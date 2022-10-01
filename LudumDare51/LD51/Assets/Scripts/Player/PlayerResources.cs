using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources instance;

    public List<GameObject> structures;
    
    public int population;
    public int builders;
    public int repairmen;


    private void Awake()
    {
        instance = this;
    }

   

    public void TakeDamage(float damage)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
