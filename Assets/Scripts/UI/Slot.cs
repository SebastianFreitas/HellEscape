using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GunOfAType gun;
    public Text gunText;


    public void UpdateGunText()
    {
        gunText.text = gun.type.ToString();
    }

    public void SwitchGun()
    {
        Debug.Log("tried");
        if (gun != null)
        {
            GameObject.FindWithTag("PlayerGun").transform.GetComponent<Gun>().SetGun(gun);
            Debug.Log("switched gun");
        }
        
    }


}
