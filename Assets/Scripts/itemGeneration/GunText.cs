using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ModBase;

public class GunText : MonoBehaviour
{
    public Text m_MyText;

    public GunOfAType gunA;

    void Start()
    {
        StartCoroutine(FindPlayer());
    }

    void Update()
    {
        //var player = GameObject.FindGameObjectsWithTag("PlayerGun");
       // gunA = player[0].transform.GetComponent<Gun>().gun1;
        //Press the space key to change the Text message
       // m_MyText.text += gunA.text;
        if (Input.GetKey(KeyCode.F))
        {
            m_MyText.text = gunA.text;
        }
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(3f);
        var player = GameObject.FindGameObjectsWithTag("PlayerGun");
        gunA = player[0].transform.GetComponent<Gun>().gunText;
        StartCoroutine(FindPlayer());

    }
}

