using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrompScript : MonoBehaviour
{

    public TextMeshProUGUI MainText, AffinityText, FundCost, DenyText, PercentageRange, ROIPercentage;
    public Image Affinity;

    public int percentageRangeA, percentageRangeB, ROIPercentageValue;

    public void Awake()
    {
        //To Do
        //Add all values to all stats

        setFundCost("500");
    }

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


    public void Fund()
    {
        GameManager.Instance.AddToCurrentList(this);
        gameObject.SetActive(false);

    }

    public void Deny()
    {
        GameManager.Instance.NextPrompt();
        Debug.Log("cago");
        gameObject.SetActive(false);
    }

    public void Copy(PrompScript obj)
    {
        obj.MainText = MainText;
        obj.AffinityText = AffinityText;
        obj.FundCost = FundCost;
        obj.DenyText = DenyText;
        obj.PercentageRange = PercentageRange;
        obj.ROIPercentage = ROIPercentage;
        obj.Affinity = Affinity;

    }
}
