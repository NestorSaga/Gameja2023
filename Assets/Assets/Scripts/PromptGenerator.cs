using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PromptGenerator : MonoBehaviour
{
    [SerializeField] TMP_Text _promptUI;
    [SerializeField] PromptData _promptData;
    private string _tag;
    private int[] _lengths = new int[5];

    void Start()
    {
        _lengths[0] = _promptData._greetings.Length;
        _lengths[1] = _promptData._identity.Length;
        _lengths[2] = _promptData._institution.Length;
        _lengths[3] = _promptData._idea.Length;
        _lengths[4] = _promptData._investment.Length;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GeneratePrompt();
        }
    }

    public void GeneratePrompt()
    {
        int[] lastPrompts = new int[5];
        int[] prompts = {Random.Range(0, _lengths[0] - 1), Random.Range(0, _lengths[1] - 1), Random.Range(0, _lengths[2] - 1), Random.Range(0, _lengths[3] - 1), Random.Range(0, _lengths[4] - 1)};
        
        for (int i = 0; i < prompts.Length; i++)
        {
            if(lastPrompts[i] == prompts[i])
            {
                prompts[i] = Random.Range(0, _lengths[i] - 1);
            }
        }
        
        _promptUI.text = _promptData._greetings[prompts[0]] + " "
                        + _promptData._identity[prompts[1]] + " "
                        + _promptData._institution[prompts[2]] + " "
                        + _promptData._idea[prompts[3]] + " "
                        + _promptData._investment[prompts[4]];
        AssignTag(prompts[3]);

        lastPrompts = prompts;
    }

    public void AssignTag(int indexIdea)
    {
        if (indexIdea == 0 || indexIdea == 1 || indexIdea == 4 || indexIdea == 5 || indexIdea == 7 || indexIdea == 8)
        {
            _tag = _promptData._tags[3];
        }
        else if (indexIdea == 2)
        {
            _tag = _promptData._tags[0];
        }
        else if (indexIdea == 3 || indexIdea == 9)
        {
            _tag = _promptData._tags[1];
        }
        else if (indexIdea == 6)
        {
            _tag = _promptData._tags[2];
        }
    }
}
