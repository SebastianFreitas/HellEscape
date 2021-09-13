using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_MyText;

    public void UpdateText(string text)
    {
        m_MyText.text = text;
    }


}

