using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHub : MonoBehaviour
{
    [SerializeField] PathFloor path;
    [SerializeField] Transform pathStartPos;

    [SerializeField] GameObject ui;

    [SerializeField] GameObject message;

    internal PathFloor currentPath;
    [SerializeField] MissionSelector selector;
    void Awake()
    {
        ui.SetActive(false);
        //selector = ;
        transform.root.GetComponent<GameMan>().currentHub = this;
    }

    internal void StartPath()
    {
        if(currentPath != null)
        {
             GameObject.Destroy(currentPath.gameObject);
        }


        currentPath = Instantiate(path, pathStartPos.position, pathStartPos.rotation, transform) as PathFloor;
        currentPath.Activate();
        currentPath.isFirst = true;
    }

    internal void Activate()
    {
        StartPath();
        ui.SetActive(true);

        if (!transform.root.GetComponent<GameMan>().startedRun)
        {
            foreach(Transform child in ui.transform) {
                child.gameObject.SetActive(true);
            }

            GetComponentInChildren<MissionSelector>().EnableSelector();

        }


        
    }

    internal void RefreshHub()
    {
        ui.SetActive(false);
        message.SetActive(true);
        if (currentPath != null)
        {
            GameObject.Destroy(currentPath.gameObject);
        }

    }
}
