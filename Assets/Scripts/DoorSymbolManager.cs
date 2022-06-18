using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSymbolManager : MonoBehaviour
{
    [SerializeField] GameObject choiceSpecial;
    [SerializeField] GameObject itemOrDrop;
    [SerializeField] GameObject shop;

    internal void UpdateSymbolMainType(RoomActivator.MainType type)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        //transform.parent.gameObject.SetActive(true);

        switch (type)
        {
            case RoomActivator.MainType.choiceSpecial:
                choiceSpecial.SetActive(true);
                break;

            case RoomActivator.MainType.itemOrDrop:
                itemOrDrop.SetActive(true);

                break;


            case RoomActivator.MainType.Shop:
                shop.SetActive(true);
                break;


        }
    
    }
   
    [SerializeField] GameObject special;
    [SerializeField] GameObject encounter;
    internal void UpdateSymbolRoomType(RoomActivator.RoomType roomType)
    {

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        //transform.parent.gameObject.SetActive(true);

        switch (roomType)
        {
            case RoomActivator.RoomType.Boss:
                {
                    
                    break;
                }

            case RoomActivator.RoomType.Encounter:
                {
                    
                    encounter.SetActive(true);
                    break;
                }

            case RoomActivator.RoomType.Special:
                {
                    special.SetActive(true);
                    break;
                }
            case RoomActivator.RoomType.Main:
                {
 
                    break;
                }
            case RoomActivator.RoomType.Corridor:
                {

                    break;
                }
            //case RoomActivator.RoomType.Trap:
            //    {

            //        break;
            //    }
            case RoomActivator.RoomType.Boon:
                {

                    break;
                }
        }
    }
}
