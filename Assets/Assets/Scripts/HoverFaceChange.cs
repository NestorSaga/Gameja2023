using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverFaceChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public int face;
    private float timeToWait = 0.05f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        //HoverTipManager.OnMouseLoseFocus();
        StartCoroutine(StartTimer2());
    }

    private void ChangeFace(int value)
    {
        GameManager.Instance.ChangeFace(value);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        ChangeFace(face);
    }
    private IEnumerator StartTimer2()
    {
        yield return new WaitForSeconds(timeToWait);
        ChangeFace(0);
    }
}