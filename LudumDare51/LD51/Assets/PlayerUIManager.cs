using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;
    private int lastKnownAvailablePop;
    private int lastKnownOverallPop;
    public TextMeshProUGUI availablePopTMP;
    public TextMeshProUGUI overallPopTMP;
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerResources.Instance.population != lastKnownAvailablePop)
        {
            lastKnownAvailablePop = PlayerResources.Instance.population;
            availablePopTMP.SetText("Available Population:\n" + lastKnownAvailablePop.ToString()); ;
        }
        if (PlayerResources.Instance.overallPopulation != lastKnownOverallPop)
        {
            lastKnownOverallPop = PlayerResources.Instance.overallPopulation;
            overallPopTMP.SetText("Overall Population:\n" + lastKnownOverallPop.ToString()); ;
        }
    }
}
