using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class StructureHealthUI : MonoBehaviour
{
    public Structure attatchedStructure;
    public Slider healthSlider;
    public Canvas theCanvas;
    private void Start()
    {
        theCanvas.transform.rotation = CameraController.instance.theCamera.transform.rotation;
        Vector3 tempscale = Vector3.one;

        tempscale.x /= transform.lossyScale.x;
        tempscale.y /= transform.lossyScale.y;
        tempscale.z /= transform.lossyScale.z;

        transform.localScale = tempscale;
    }

    public void SetHealthCount(float current, float max)
    {
        healthSlider.value = current;
        healthSlider.maxValue = max;
    }
}
