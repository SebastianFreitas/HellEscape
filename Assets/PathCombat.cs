using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCombat : MonoBehaviour
{
    internal Bounds combatBounds;

    internal int maxsteps;
    int totalMobs = 10;
    [SerializeField] Monster skull;

    private void Start()
    {
        GetBounds();

        

        for(int i = 0; i < totalMobs; i++)
        {

            Vector3 randomPoint = RandomPointInBounds(combatBounds);
            Monster currentMob = Instantiate(skull, randomPoint, Quaternion.identity, transform) as Monster;
            currentMob.isHub = true;
        }
    }

    public void GetBounds()
    {
        combatBounds = new Bounds(transform.position, Vector3.one);

        var collider = transform.GetComponent<BoxCollider>();

        combatBounds.Encapsulate(collider.bounds);
    }

    private Vector3 RandomPointInBounds(Bounds rawBounds)
    {
        Bounds bounds = rawBounds;
        bounds.size /= 2;
        return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    internal void IsEncounterDone()
    {
        totalMobs--;

        if(totalMobs < 1)
        {
            OpenPath();
        }
    }

    private void OpenPath()
    {
        var x = GetComponentInChildren<PathFloor>();
        x.isActive = true;
        x.maxSteps = maxsteps--;
    }
}
