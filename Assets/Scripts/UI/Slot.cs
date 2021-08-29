using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler
{
    public GunOfAType gun;
    public Text gunType;
    public GunText uiText;

    public GameObject ButtonGameObject;


    public void Update()
    {
        // Compare selected gameObject with referenced Button gameObject
        if (EventSystem.current.currentSelectedGameObject == ButtonGameObject)
        {
            if (Input.GetKeyDown("e"))  GetComponent<Button>().onClick.Invoke();
            
        }
    }

    public void UpdateGunText()
    {
        gunType.text = gun.type.ToString();
        uiText.gameObject.SetActive(true);
    }

    public void SwitchGun()
    {
        if (gun != null)
        {
            GameObject.FindWithTag("PlayerGun").transform.GetComponent<Gun>().SetGun(gun);
        }
        
    }

    public void ShowGun()
    {
        if (gun != null)
        {
            uiText.UpdateText(gun.text);
            StartCoroutine(FadeGunText());
        }
        
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        ShowGun();
    }

    IEnumerator FadeGunText()
    {
        yield return new WaitForSeconds(10f);
        uiText.gameObject.SetActive(false);
    }
}
