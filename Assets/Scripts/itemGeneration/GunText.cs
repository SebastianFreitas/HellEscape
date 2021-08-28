using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ModBase;

public class GunText : MonoBehaviour
{
    public Text m_MyText;

    public void UpdateText(string text)
    {
        m_MyText.text = text;
    }


}

