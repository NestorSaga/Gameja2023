using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int currentGold;
    public TextMeshProUGUI currentGoldText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recalculateCurrentGold(int value)
    {
        currentGold += value;
        currentGoldText.text = currentGold.ToString();
    }
}
