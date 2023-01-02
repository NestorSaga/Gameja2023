using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PromptGenerator : MonoBehaviour
{
    [SerializeField] TMP_Text _promptUI;
    [SerializeField] PromptData _promptData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            GeneratePrompt();
        }
    }

    public void GeneratePrompt()
    {
        _promptUI.text = _promptData._greetings[Random.Range(0, _promptData._greetings.Length - 1)] + " "
                        + _promptData._identity[Random.Range(0, _promptData._identity.Length - 1)] + " "
                        + _promptData._institution[Random.Range(0, _promptData._institution.Length - 1)] + " "
                        + _promptData._idea[Random.Range(0, _promptData._idea.Length - 1)] + " "
                        + _promptData._investment[Random.Range(0, _promptData._investment.Length - 1)];                        
    }
}
