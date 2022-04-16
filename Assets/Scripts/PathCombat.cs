using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCombat : PathAux
{
    internal Bounds combatBounds;

    internal int maxsteps;
    [SerializeField]  internal int totalMobs = 10;
    [SerializeField] Monster[] mobs;
    [SerializeField] PathFloor path;
    [SerializeField] Transform nextSteps;

    [SerializeField] GameObject combatSymbol;

    internal int dificulty;
    internal int addedLife;
    internal int sizeMultiplier = 0;
    internal PathFloor.EncounterType type;
    [SerializeField] private SpikeTrap spikes;

    private void Start()
    {
        GetBounds();

        // totalMobs += GetComponentInParent<PathFloor>().dificulty;

        var chance = 90 - dificulty;
        if (chance < 65) chance = 65;
        if (Random.Range(1, 101) > chance) spikes.gameObject.SetActive(true);
        else spikes.gameObject.SetActive(false);
    }



    internal IEnumerator Waiter()
    {
        yield return new WaitForFixedUpdate();
        List<int> list = new List<int>();
       // var numberList = Enumerable.Range(1, 10).Select().ToList();

        if (totalMobs < 1) totalMobs = 1;
        if (totalMobs > 10) totalMobs = 10;
        for (int i = 0; i < totalMobs; i++)
        {
            var x = 0;
            if (i > 0) x = Random.Range(0, mobs.Length);
            Vector3 randomPoint = RandomPointInBounds(combatBounds);
            Monster currentMob = Instantiate(mobs[x], randomPoint, Quaternion.identity, transform) as Monster;
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
   
        newPtah.maxSteps = maxsteps--;
        newPtah.Activate();


        combatSymbol.SetActive(false);
    }
}
