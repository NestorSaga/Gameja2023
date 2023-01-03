using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PromptGenerator : MonoBehaviour
{
    [SerializeField] TMP_Text _promptUI;
    [SerializeField] PromptData _promptData;
    public string _tag;
    public int _colorId;
    public int[] _lengths = new int[4];

    public void GeneratePrompt()
    {

        _lengths[0] = _promptData._greetings.Length;
        _lengths[1] = _promptData._identity.Length;
        _lengths[2] = _promptData._institution.Length;
        _lengths[3] = _promptData._idea.Length;

        int[] lastPrompts = new int[4];
        int[] prompts = {Random.Range(0, _lengths[0] - 1), Random.Range(0, _lengths[1] - 1), Random.Range(0, _lengths[2] - 1), Random.Range(0, _lengths[3] - 1)};

        for (int i = 0; i < prompts.Length; i++)
        {
            do
            {
                prompts[i] = Random.Range(0, _lengths[i] - 1);
            } while (lastPrompts[i] == prompts[i]);
        }
        
        _promptUI.text = _promptData._greetings[prompts[0]] + " "
                        + _promptData._identity[prompts[1]] + " "
                        + _promptData._institution[prompts[2]] + " "
                        + _promptData._idea[prompts[3]] + " ";



        _tag = _promptData._tags[prompts[3]];
        _colorId = prompts[3];


        lastPrompts = prompts;
    }
}
