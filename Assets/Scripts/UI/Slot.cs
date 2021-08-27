using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GunOfAType gun;
    public Text gunText;

    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGunText(string text)
    {
        gunText.text = text;
    }

    void Hide()
    {
        GetComponent<CanvasRenderer>().SetAlpha(0f); //this makes everything transparent
        //canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }

    public void Show()
    {
        GetComponent<CanvasRenderer>().SetAlpha(1f); //this makes everything transparent
        //canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }


}
