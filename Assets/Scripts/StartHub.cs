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
    void Awake()
    {
        ui.SetActive(false);

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

        foreach(Transform child in ui.transform) {
            child.gameObject.SetActive(true);
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
