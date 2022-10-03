using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public Color attackerColor = Color.white;
    public Color repairColor = Color.green;
    public Color builderColor = Color.yellow;

    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponentInChildren<SpriteRenderer>();

        Vector3 tempscale = Vector3.one;

        tempscale.x /= transform.lossyScale.x;
        tempscale.y /= transform.lossyScale.y;
        tempscale.z /= transform.lossyScale.z;

        transform.localScale = tempscale;
    }
    public void SetRange(float p_range, IndicatorType type)
    {
        spr.transform.localScale = Vector3.one * p_range;
        switch (type)
        {
            case IndicatorType.attacker:
                spr.color = attackerColor;
                break;
            case IndicatorType.builder:
                spr.color = builderColor;
                break;
            case IndicatorType.repairer:
                spr.color = repairColor;
                break;
        }
    }
    public enum IndicatorType
    {
        attacker, builder, repairer
    }
}
