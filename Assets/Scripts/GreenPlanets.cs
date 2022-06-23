using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlanets : MonoBehaviour
{
    [SerializeField] GameObject planet;
    void Start()
    {
        var boxCollider = transform.GetComponent<BoxCollider>();
        planet.SetActive(true);
        for (int i = 0; i < 100; i++)
        {
            //var pos = new Vector3(Random.Range(1000,99999), Random.Range(1000, 99999), Random.Range(1000, 99999));
            var newPlanet = Instantiate(planet);


            float minRad = 999; //set this to the radius of the smallest sphere
            float maxRad = 9999; //set this to the radius of the largest sphere
            float rx = Random.Range(-1f, 1f) * 2f * Mathf.PI; //rand() is a generic pseudofunction that should generate a value between 0.0f and 1.0f
            float ry = Random.Range(-1f, 1f) * 2f * Mathf.PI;
            float rz = Random.Range(-1f, 1f) * 2f * Mathf.PI;
            Vector3 angle = new Vector3(rx, ry, rz);
            float radius = minRad + Random.Range(0.0f, 1f) * (maxRad - minRad);

            //Vector3 rpos = 





            newPlanet.transform.position = angle * radius;
            newPlanet.transform.localScale *= Random.Range(0.0001f, .5f);

            //newPlanet.GetComponent<MeshFilter>().sharedMesh = mesh;
            newPlanet.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
            newPlanet.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        }
        planet.SetActive(false);
    }

    private Vector3 RandomPointInBounds(Bounds rawBounds)
    {
        Bounds bounds = rawBounds;
        return new Vector3(
            Random.Range(999, bounds.max.x),
            Random.Range(999, bounds.max.y),
            Random.Range(999, bounds.max.z)
        );
    }
}
