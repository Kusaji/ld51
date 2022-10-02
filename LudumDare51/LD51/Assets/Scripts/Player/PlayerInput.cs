using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameObject objectClickedOn;
    public Structure structure;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetClickedOnObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            GetOnClickUpObject();
        }
        if (Input.GetMouseButtonDown(1))
        {
            GetRightClickedOnObject();
        }

        if (Input.GetMouseButtonUp(1))
        {
            GetOnRightClickUpObject();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && PlayerStructures.instance.spawningTower)
        {
            PlayerStructures.instance.spawningTower = false;
            Destroy(PlayerStructures.instance.inactiveTower);
        }
    }

    public void GetClickedOnObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            objectClickedOn = hit.transform.gameObject;
        }

        if (objectClickedOn.CompareTag("Structure"))
        {
            structure = hit.transform.gameObject.GetComponent<StructureHitbox>().structure;
            structure.OnClickDown();
        }
    }

    public void GetOnClickUpObject()
    {
        if (structure != null)
        {
            structure.OnClickUp();
            structure = null;
        }
    }
    public void GetRightClickedOnObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            objectClickedOn = hit.transform.gameObject;
        }

        if (objectClickedOn.CompareTag("Structure"))
        {
            structure = hit.transform.gameObject.GetComponent<StructureHitbox>().structure;
            structure.OnRightClickDown();
        }
    }

    public void GetOnRightClickUpObject()
    {
        if (structure != null)
        {
            structure.OnRightClickUp();
            structure = null;
        }
    }
}
