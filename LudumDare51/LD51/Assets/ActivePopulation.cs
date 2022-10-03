using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActivePopulation : MonoBehaviour
{
    public Structure attatchedStructure;
    public TextMeshProUGUI populationTMP;
    public Canvas theCanvas;
    public GraphicRaycaster graycaster;
    private void Awake()
    {
        GetComponentInChildren<CustomButton>().activePop = this;
    }
    private void Start()
    {
        theCanvas.transform.rotation = CameraController.instance.theCamera.transform.rotation;
        //graycaster.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        //theCanvas.transform.LookAt(CameraController.instance.theCamera.transform);
        //Vector3 lookie = theCanvas.transform.eulerAngles;
        //theCanvas.transform.rotation = Quaternion.Euler(lookie.x + 180f, lookie.y, lookie.z + 180f);
        
        
        
    }
    public void SetPopulationCount(int active, int minimumRequired)
    {
        //  Display effectiveness somewhere?
        //populationTMP.SetText(active + " / " + minimumRequired);
        populationTMP.SetText(active.ToString());
    }
    public void ClickDown()
    {
        attatchedStructure.OnClickDown();
    }
    public void ClickUp()
    {
        attatchedStructure.OnClickUp();
    }
}
