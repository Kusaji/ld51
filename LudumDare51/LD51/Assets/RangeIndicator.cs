using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public Structure myStructure;
    public Color attackerColor = Color.white;
    public Color repairColor = Color.green;
    public Color builderColor = Color.yellow;
    public Color clearColor = Color.clear;
    public SpriteRenderer spr;
    private IndicatorType myType = IndicatorType.attacker;
    public Color desiredColor
    {
        get
        {
            switch (myType)
            {
                case IndicatorType.attacker:
                    return attackerColor;
                case IndicatorType.builder:
                    return builderColor;
                case IndicatorType.repairer:
                    return repairColor;
            }
            return attackerColor;
        }
    }
    private void Awake()
    {
        Vector3 tempscale = Vector3.one;

        tempscale.x /= transform.lossyScale.x;
        tempscale.y /= transform.lossyScale.y;
        tempscale.z /= transform.lossyScale.z;

        transform.localScale = tempscale;
    }
    private void Update()
    {
        if (myStructure != null)
        {
            if (myStructure.iAmHighlighted)
                SmoothFunc.Damp(spr.color, desiredColor, 0.000001f, Time.unscaledDeltaTime);
            else
                SmoothFunc.Damp(spr.color, Color.clear, 0.000001f, Time.unscaledDeltaTime);

        }
    }
    public void SetRange(float p_range, IndicatorType type)
    {
        myType = type;
        spr.transform.localScale = Vector3.one * p_range;
        spr.color = desiredColor;
    }
    public enum IndicatorType
    {
        attacker, builder, repairer
    }
}
