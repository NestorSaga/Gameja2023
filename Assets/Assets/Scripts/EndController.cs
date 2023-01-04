using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndController : MonoBehaviour
{
    public RectTransform game;
    public RectTransform lose;

    public Canvas UICanvas, characterCanvas, endCanvas;

    [TextArea(15, 20)]
    public string loseMessage;

    [TextArea(15, 20)]
    public string winMessage;

    public TextMeshProUGUI endText;

    public void GameEnd(bool win)
    {
        UICanvas.gameObject.SetActive(false);
        characterCanvas.gameObject.SetActive(false);

        if (win) endText.text = winMessage;
        else endText.text = loseMessage;
        //game.gameObject.SetActive(false);
        //lose.gameObject.SetActive(true);
    }

    public void replay()
    {
        SceneManager.LoadScene(1);
    }

    public void menu()
    {
        SceneManager.LoadScene(0);
    }
}
