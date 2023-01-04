using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStashManager : MonoBehaviour
{

    GameObject[] coins;

    int nCoinsEnabled = 0;
    int maxAmount = 1_000_000;

    [SerializeField] AnimationClip coin_IN, coin_OUT;

    //private void Start()
    //{
    //    foreach (RectTransform rt in GetComponentsInChildren<RectTransform>()) {
    //
    //        coins[nCoinsEnabled] = rt.GetComponentInChildren<GameObject>();
    //        nCoinsEnabled++;
    //    }
    //    Debug.Log(coins);
    //}

    public void SetAmount(int amount) {

        int ratio = amount / maxAmount * coins.Length;

        if (amount < nCoinsEnabled) {
            for (int i = nCoinsEnabled; i >= ratio; i--)
            {
                coins[i].GetComponent<Animation>().clip = coin_OUT;
                coins[i].GetComponent<Animation>().Play();
                //AÑADIR SONIDO
            }
        }

        if (amount < nCoinsEnabled) {
            for (int i = nCoinsEnabled; i < ratio; i++)
            {
                coins[i].GetComponent<Animation>().clip = coin_IN;
                coins[i].GetComponent<Animation>().Play();
                //AÑADIR SONIDO
            }
        }
    }

}
