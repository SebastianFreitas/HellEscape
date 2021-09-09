using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class HighLightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI buttonText;

    private string formatedString = "{value}";

    private void Start()
    {
        formatedString += buttonText.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.text = formatedString.Replace("{value}", "-");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.text = formatedString.Replace("{value}", "");
    }
}
