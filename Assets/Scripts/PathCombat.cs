using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCombat : MonoBehaviour
{
    internal Bounds combatBounds;

    internal int maxsteps;
    [SerializeField]  internal int totalMobs = 10;
    [SerializeField] Monster skull;
    [SerializeField] PathFloor path;
    [SerializeField] Transform nextSteps;

    [SerializeField] GameObject combatSymbol;

    internal int dificulty;
    internal int addedLife;
    internal int sizeMultiplier = 0;
    internal PathFloor.EncounterType type;
    private void Start()
    {
        GetBounds();

        // totalMobs += GetComponentInParent<PathFloor>().dificulty;

        
    }

    internal IEnumerator Waiter()
    {
        yield return new WaitForFixedUpdate();

        if (totalMobs < 1) totalMobs = 1;
        for (int i = 0; i < totalMobs; i++)
        {

            Vector3 randomPoint = RandomPointInBounds(combatBounds);
            Monster currentMob = Instantiate(skull, randomPoint, Quaternion.identity, transform) as Monster;
            currentMob.isHub = true;

            currentMob.health += addedLife;
            if (sizeMultiplier > 0) currentMob.TurnBig();
            else if (sizeMultiplier < 0) currentMob.TurnMini();
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
        PathFloor newPtah = Instantiate(path, nextSteps.position, nextSteps.rotation, transform) as PathFloor;

        newPtah.Activate();


        combatSymbol.SetActive(false);
    }
}
