using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivePopulation : MonoBehaviour
{
    public Structure attatchedStructure;
    public TextMeshProUGUI populationTMP;
    public Canvas theCanvas;
    private void Start()
    {
        theCanvas.transform.rotation = CameraController.instance.theCamera.transform.rotation;
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
}
