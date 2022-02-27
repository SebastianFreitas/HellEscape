using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragments : MonoBehaviour
{

    [SerializeField] GameObject[] objetcts;
    [SerializeField] int numObjects = 10;
    [SerializeField] int numObjectsLine = 10;
    [SerializeField] float radius;


    private void OnEnable()
    {
        Vector3 center = transform.position;

        for (int i = 0; i < numObjects; i++)
        {
            Vector3 pos = RandomCircle(center, radius);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
            var obj = Instantiate(objetcts[Random.Range(0, objetcts.Length)], pos, rot, transform);
            obj.transform.Rotate(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
        }


        for (int i = 0; i < numObjectsLine; i++)
        {
            Vector3 pos = center + new Vector3(0, 0, i+30);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
            var obj = Instantiate(objetcts[Random.Range(0, objetcts.Length)], pos, rot, transform);
            obj.transform.Rotate(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
        }
    }
    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
