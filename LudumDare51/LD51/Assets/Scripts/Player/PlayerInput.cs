using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player Input.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    #region Variables
    [Header("Singleton")]
    public static PlayerInput Instance;
    
    [Header("Runtime References")]
    public GameObject objectClickedOn;
    public Structure structure;
    public Vector2 testMatrix = Vector2.one;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        Instance = this;
    }
    public static Vector3 ScaledMousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }
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
    #endregion

    #region Methods
    public void GetClickedOnObject()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = CameraController.instance.theCamera.ScreenPointToRay(ScaledMousePosition);
        

        if (Physics.Raycast(ray, out hit))
        {
            objectClickedOn = hit.transform.gameObject;
        }

        if (objectClickedOn != null && objectClickedOn.CompareTag("Structure"))
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
        Ray ray = Camera.main.ScreenPointToRay(ScaledMousePosition);

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
    #endregion
}
