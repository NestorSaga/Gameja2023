using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PromptGenerator : MonoBehaviour
{

    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] Lines = System.IO.File.ReadAllLines("/* YOUR_PATH   */");
        //string[] Columns= Lines[/*   INDEX  */].Split(';');
        for (int i=0; i<=Lines.Length-1; i++)
        {
            Debug.Log(Lines[i]);
        }
    }

    public string GeneratePrompt()
    {
        return null;
    }
}
