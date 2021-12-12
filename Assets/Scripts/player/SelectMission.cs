using System;
using UnityEngine;

public class SelectMission : MonoBehaviour
{
    public MissionSelector selector;
    internal void OpenPortal()
    {
        selector.OpenPortal();
    }
}