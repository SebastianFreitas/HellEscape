using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonDrop : MonoBehaviour
{
    private VoidBoon boon;
    public TMPro.TextMeshPro text;

    [SerializeField] bool isInfluencePool = false;

    private void Awake()
    {
        boon = transform.root.GetComponent<GameMan>().boonManager;
        boon.RefreshBoon();

        text.text = boon.text;
    }

    internal void Accept()
    {
        boon.AcceptOrRemoveBoon(true);
        boon.RemoveBoon();

        gameObject.SetActive(false);

 

        //text.text = boon.text;
    }


  
    internal void Deny()
    {
        boon.playerHP.TakeDamage(10);
        gameObject.SetActive(false);
    }
}
