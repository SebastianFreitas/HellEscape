using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToDesapear : MonoBehaviour
{
    [SerializeField] int chance;
    void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            if (Random.Range(1, 101) > 100- chance) child.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame

}
