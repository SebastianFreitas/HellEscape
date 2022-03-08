using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class HighLightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI buttonText;

    private string labeltext;

    private string unselected;
    private string selected;

    private void Awake()
    {
        labeltext = buttonText.text;
        unselected = " " + labeltext;
        selected = "<" + labeltext + ">";
        buttonText.text = unselected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.text = selected;// formatedString.Replace("{value}", "-");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.text = unselected;//formatedString.Replace("{value}", "");
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        buttonText.text = unselected;
    }
}
