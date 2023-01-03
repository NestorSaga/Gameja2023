using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatController : MonoBehaviour
{
    public RectTransform game;
    public RectTransform lose;

    public void GameLost()
    {
        game.gameObject.SetActive(false);
        lose.gameObject.SetActive(true);
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
