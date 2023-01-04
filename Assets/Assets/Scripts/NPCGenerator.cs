using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCGenerator : MonoBehaviour
{
    public Sprite[] heads, masks, torsos, legs;
    public int []lastIndex = new int[4];

    public AnimationClip animEnter, animExit;
    public Animation anim;



    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) GenerateNPC();
    }

    public void GenerateNPC()
    {
       /* for (int i = 0; i<4;i++)
        {
            lastIndex[i] = Random.Range(0, array.Length);
            transform.GetChild(i).GetComponent<Image>().sprite = RandomFromArray(heads);
        }*/



        transform.GetChild(0).GetComponent<Image>().sprite = RandomFromArray(heads);
        transform.GetChild(1).GetComponent<Image>().sprite = RandomFromArray(masks);
        transform.GetChild(2).GetComponent<Image>().sprite = RandomFromArray(torsos);
        transform.GetChild(3).GetComponent<Image>().sprite = RandomFromArray(legs);
    }

    public Sprite RandomFromArray(Sprite[] array)
    {
        int random = Random.Range(0, array.Length);
        
        return array[random];
    }

    public void PlayAnimation(bool isEntering)
    {
        if (isEntering)
        {
            anim.clip = animEnter;
            anim.Play();
        }
        else
        {
            anim.clip = animExit;
            anim.Play();
        }
    }
}
