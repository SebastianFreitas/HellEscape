using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ButtonThroughKeySelection : MonoBehaviour
{

    public string key;

    public void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

        if (Input.GetKeyDown("o") )
        {
          
            GetComponent<Button>().onClick.Invoke();
            
        }


    }
}
