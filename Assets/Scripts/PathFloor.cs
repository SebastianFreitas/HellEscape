using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFloor : PathAux
{
    [SerializeField] internal bool isActive = true;
    private bool passed = false;
    [SerializeField] Transform[] nextSteps;
    [SerializeField] Transform forward;
    [SerializeField] GameObject mainBase;

    [SerializeField] PathFloor path;
    //[SerializeField] GameObject mainBaseUI;
    [SerializeField] GameObject[] additionalObjects;
    [SerializeField] internal int maxSteps = 0;

    [SerializeField] GameObject arrow;

    [SerializeField] GameObject adds;
    [SerializeField] bool isAdds = false;
    internal void Activate()
    {
        isActive = true;
        arrow.SetActive(true);
    }



    private int totalSteps;
    public int dificulty = 0;
    [SerializeField] PathCombat[] combats;

    [SerializeField]  private SpikeTrap spikes;

    private int encounterConter;

    private void Start()
    {

        adds.SetActive(isAdds);
        
        if (PlayerPrefs.HasKey("PathLevel"))
        {
            dificulty = PlayerPrefs.GetInt("PathLevel");
        }
        if (isFirst)  maxSteps += 10 * dificulty;
        totalSteps = maxSteps;
        if (isActive) arrow.SetActive(true);

        var chance = 90 - dificulty;
        if (chance < 65) chance = 65;
        if (Random.Range(1, 101) > chance) spikes.gameObject.SetActive(true);
        else spikes.gameObject.SetActive(false);
    }
   internal bool victory = false;
    [System.Obsolete]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude") && !passed && isActive)
        {
            passed = true;
            arrow.SetActive(false);


            if (maxSteps == 0)
            {
                maxSteps--;
                dificulty++;
                SaveDificulty();
                var x = Instantiate(mainBase, right.position, right.rotation, transform);
                x.transform.parent = GetComponentInParent<Hub>().transform; 
                victory = true;
                var par = transform.parent;
                transform.parent = x.transform;
                Destroy(par.GetComponentInParent<StartHub>().gameObject);
            } 
            else if (IsDivisible(maxSteps, 10))
            {
                if(maxSteps<5 * dificulty) SpawnCombat(EncounterType.Hard);
                else SpawnCombat(EncounterType.Normal);
            }
            else if (maxSteps > 0)
            {
                PathFloor a = new PathFloor();
                Transform b = forward;
                bool islazer = false;
                var rando = Random.Range(0, 100);
                if (rando > 85)
                {
                    SpawnCombat(EncounterType.Easy);
                }
                else 
                { 
                    if (rando > 75-dificulty)
                    {
                        b = nextSteps[Random.Range(0, nextSteps.Length)];
                        islazer = true;
                    }

                    a = Instantiate(path, b.position, b.rotation, transform) as PathFloor;
                    a.maxSteps = maxSteps - 1;
                    a.isAdds = false;
                    a.isFirst = false;

                    //laser.gameObject.SetActive(true);
                    //laser2.gameObject.SetActive(true);
                    //if (islazer)
                    //{
                    //    laser.transform.LookAt(a.lasertarget);
                    //    laser2.transform.LookAt(a.lasertarget2);
                    //}
                    //else
                    //{
                    //    laser.transform.LookAt(Vector3.zero);
                    //    laser2.transform.LookAt(Vector3.zero);
                    //}
                }

            }

        }
    }
    internal enum EncounterType
    {
        Easy,
        Normal,
        Hard,
        Impossible
    }
    [SerializeField] Transform forwardPlus;
    [SerializeField] Transform forwardPlusUP;
    [SerializeField] Transform leftPlusUP;
    [SerializeField] Transform rightPlusUP;
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    internal bool isFirst= false;

    private void SpawnCombat(EncounterType type)
    {
        PathCombat a = new PathCombat();

        //laser.transform.LookAt(Vector3.zero);
        //laser2.transform.LookAt(Vector3.zero);
        if (dificulty == 0) type = EncounterType.Easy;
        switch (type)
        {
            case EncounterType.Easy:
                a = Instantiate(combats[Random.Range(0, combats.Length)], forward.position, forward.rotation, transform) as PathCombat;



                a.totalMobs = Random.Range(1, dificulty/5);
                a.maxsteps = maxSteps - 1;
                a.dificulty = dificulty;
                a.StartCoroutine(a.Waiter());
                break;

            case EncounterType.Normal:
                a = Instantiate(combats[Random.Range(0, combats.Length)], forward.position, forward.rotation, transform) as PathCombat;


                var total = Random.Range(dificulty/5, 2 * (dificulty/5));
                if (Random.Range(0, 100) > 50)
                {
                    a.totalMobs = total;
                    a.sizeMultiplier += -1;
                }
                else
                {
                    a.totalMobs = total / 2;
                    a.sizeMultiplier += 1;
                    a.addedLife += 10 * dificulty;
                }
                a.maxsteps = maxSteps - 1;
                a.dificulty = dificulty;
                a.StartCoroutine(a.Waiter());

                break;

            case EncounterType.Hard:

                a = SpawnNormal(ref forward);
                laser.transform.LookAt(a.lasertarget);
                laser2.transform.LookAt(a.lasertarget2);

                a = SpawnNormal(ref left);

                laser.transform.LookAt(a.lasertarget);
                laser2.transform.LookAt(a.lasertarget2);
                a = SpawnNormal(ref right);

                laser.transform.LookAt(a.lasertarget);
                laser2.transform.LookAt(a.lasertarget2);

                break;

            case EncounterType.Impossible:

                break;
        }


    }

    private PathCombat SpawnNormal(ref Transform pos)
    {
        PathCombat a = Instantiate(combats[Random.Range(0, combats.Length)], pos.position, pos.rotation, transform) as PathCombat;
        var total = Random.Range(dificulty / 5, 2 * (dificulty / 5));
        if (Random.Range(0, 100) > 50)
        {
            a.totalMobs = total;
        }
        else
        {
            a.totalMobs = total / 2;
            a.sizeMultiplier += 1;
            a.addedLife += 10 * dificulty;
        }
        a.maxsteps = maxSteps - 1;
        a.dificulty = dificulty;
        a.StartCoroutine("Waiter");
        return a;
    }

    public bool IsDivisible(int x, int n)
    {
        return (x % n) == 0;
    }

    internal void ResetPath()
    {
        if (!victory)
        {

            //GetComponentInParent<StartHub>().StartPath();
            Destroy(this);

            passed = false;
            maxSteps = totalSteps;
            int i = 0;

            foreach (Transform child in this.transform)
            {
                if (i > 6)GameObject.Destroy(child.gameObject);
                i++;
            }

            laser.gameObject.SetActive(false);
            laser2.gameObject.SetActive(false);
        }


    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("PathLevel"))
        {
            dificulty = PlayerPrefs.GetInt("PathLevel");
        }

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"),false);
    }

    private void OnDisable()
    {
        //SaveDificulty();
    }

    private void SaveDificulty()
    {
        PlayerPrefs.SetInt("PathLevel", dificulty);
    }
}
