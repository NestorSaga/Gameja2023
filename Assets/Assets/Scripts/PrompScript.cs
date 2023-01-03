using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PrompScript : MonoBehaviour
{

    public TextMeshProUGUI MainText, FundCost, DenyText, PercentageRange, ROIPercentage;
    public Sprite[] affinity_shadows;
    public Color[] affinity_colors;
    public Image Affinity, shadow;

    public int percentageRangeA, percentageRangeB, fundCostValue;
    public float ROIPercentageValue;
    public bool hasWon;

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
    
    public void setAffinityColor(int index)
    {
        Affinity.color = affinity_colors[index];
    }

    public void setAffinityShadow(int id)
    {
        shadow.sprite = affinity_shadows[id];
    }

    public void setFundCost(string text)
    {
        fundCostValue = int.Parse(text);
        FundCost.text = text;
    }

    public void setDenyText(string text)
    {
        DenyText.text = text;
    }

    public void setPercentageRange(int a, int b)
    {
        percentageRangeA = a;
        percentageRangeB = b;
        string text = a + " - " + b;
        PercentageRange.text = text;
    }

    public void setROIPercentage(float a)
    {
        ROIPercentageValue = a;
        string text = a * 100 + "%";
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
        
        gameObject.SetActive(false);
    }

    
}
