using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStructures : MonoBehaviour
{
    public static PlayerStructures instance;

    [Header("Active Structures")]
    public List<GameObject> structures;

    [Header("Available Structures")]
    public List<GameObject> structurePrefabs;

    public Vector3 mousePosition;


    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuildStructure();
        }
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
    }

    public void RemoveStructure(GameObject structure)
    {
        structures.Remove(structure);
    }

    public void BuildStructure()
    {
        GetMousePosition();

        var builtStructure = Instantiate(
            structurePrefabs[0],
            mousePosition,
            Quaternion.identity
            );

        AddStructure(builtStructure);
    }

    public void GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
        }
    }
}
