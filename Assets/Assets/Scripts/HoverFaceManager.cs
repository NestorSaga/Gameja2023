using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverFaceManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;
    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 position)
    {
        tipText.text = tip;
        //tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        //tipWindow.anchoredPosition = new Vector2(position.x, position.y);
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
