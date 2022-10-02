using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTower : MonoBehaviour
{
    public List<Structure> towersInRange;

    [Header("Can heal self?")]
    public bool canHealSelf;

    public float towerHealRange;
    public float towerHealAmount;
    public int activeHealTargets;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetTowersInRange();
        }
    }

    public void GetTowersInRange()
    {
        if (PlayerStructures.instance.structures.Count > 0)
        {
            
            for (int i = 0; i < PlayerStructures.instance.structures.Count; i++)
            {
                if (!canHealSelf)
                {
                    if (PlayerStructures.instance.structures[i].gameObject != gameObject) //Check to make sure we don't add ourself to list.
                    {
                        if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) <= towerHealRange)
                        {
                            towersInRange.Add(PlayerStructures.instance.structures[i].GetComponent<Structure>());
                        }
                    }
                }
                else if (canHealSelf) //Bypass check and have ourself on list to be healed.
                {
                    if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) <= towerHealRange)
                    {
                        towersInRange.Add(PlayerStructures.instance.structures[i].GetComponent<Structure>());
                    }
                }
            }
        }
    }

}
