using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHub : MonoBehaviour
{
    [SerializeField] PathFloor path;
    [SerializeField] Transform pathStartPos;

    internal PathFloor currentPath;
    void Start()
    {
        StartPath();
    }

    internal void StartPath()
    {
        if(currentPath != null)
        {
             GameObject.Destroy(currentPath.gameObject);
        }


        currentPath = Instantiate(path, pathStartPos.position, pathStartPos.rotation, transform) as PathFloor;
        currentPath.isActive = true;
    }


}
