using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorButtons : MonoBehaviour
{

    [SerializeField] MirrorManager.MirrorBoon boonType;
    [SerializeField] int operatoR;
    internal bool PushButton()
    {
        return GetComponentInParent<MirrorManager>().InsertPoint(boonType, operatoR);
    }
}
