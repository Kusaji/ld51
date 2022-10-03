using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelizerFadeIn : MonoBehaviour
{
    public Color fadedColor;
    private MeshRenderer mr;
    float fadeInTime = 0.15f;
    float startTime;
    
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.material = mr.material;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        mr.sharedMaterial.SetColor("_BaseColor", Color.Lerp(fadedColor, Color.white, (Time.time - startTime)));
    }
}
