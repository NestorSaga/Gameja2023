using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrompScript : MonoBehaviour
{

    public TextMeshProUGUI MainText, AffinityText, FundCost, DenyText, PercentageRange, ROIPercentage;
    public Image Affinity;


    public void setMainText(string text)
    {
        MainText.text = text;
    }
    
    public void setAffinityText(string text)
    {
        AffinityText.text = text;
    }

    public void setFundCost(string text)
    {
        FundCost.text = text;
    }

    public void setDenyText(string text)
    {
        DenyText.text = text;
    }

    public void setPercentageRange(int a, int b)
    {
        string text = a + " - " + b;
        PercentageRange.text = text;
    }

    public void setROIPercentage(int a)
    {
        string text = a + "%";
        ROIPercentage.text = text;
    }
}
