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
        //boon = new VoidBoon(GetComponentInParent<RoomActivator>().influcence);


        //text.text = boon.text;
    }

    private void OnEnable()
    {
        boon = new VoidBoon();

        text.text = boon.text;
    }

    internal void Accept()
    {
        boon.AcceptOrRemoveBoon(true);
        //gameObject.SetActive(false);
        boon.RefreshBoon();

        text.text = boon.text;
    }


  
    internal void Deny()
    {
        boon.playerHP.TakeDamage(5);
        gameObject.SetActive(false);
    }
}
