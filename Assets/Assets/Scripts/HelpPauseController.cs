using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPauseController : MonoBehaviour
{
    public RectTransform helpMenu;

    private void Start()
    {
        helpMenu.gameObject.SetActive(false);
    }
    public void PauseMenu()
    {
        
    }

    public void HelpMenu()
    {
        if (helpMenu.gameObject.activeInHierarchy == true)
        {
            helpMenu.gameObject.SetActive(false);
        }
        else if (helpMenu.gameObject.activeInHierarchy == false)
        {
            helpMenu.gameObject.SetActive(true);
        }
    }
}
