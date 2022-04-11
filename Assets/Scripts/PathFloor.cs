using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFloor : MonoBehaviour
{
    [SerializeField] internal bool isActive = true;
    private bool passed = false;
    [SerializeField] Transform[] nextSteps;
    [SerializeField] Transform forward;
    [SerializeField] GameObject mainBase;

    [SerializeField] PathFloor path;
    //[SerializeField] GameObject mainBaseUI;
    [SerializeField] GameObject[] additionalObjects;
    [SerializeField] internal int maxSteps = 40;

    [SerializeField] Transform laser;
    [SerializeField] Transform lasertarget;

    [SerializeField] Transform laser2;
    [SerializeField] Transform lasertarget2;

    private int totalSteps;
    internal int dificulty = 0;
    [SerializeField] PathCombat[] combats;

    private int encounterConter;

    private void Start()
    {

        maxSteps += 10 * dificulty;
        totalSteps = maxSteps;
        //laser.gameObject.SetActive(false);
        //for(int i = Random.Range(-18, 2); i > 0; i--)
        //{
        //    additionalObjects[Random.Range(0, additionalObjects.Length)].SetActive(true);
        //}

    }
    bool victory = false;
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !passed && isActive)
        {
            passed = true;

            if (maxSteps == 0)
            {
                var b = nextSteps[Random.Range(0, nextSteps.Length)];
                var x = Instantiate(mainBase, b.position, b.rotation, transform);
                transform.parent = transform.parent;
                victory = true;
                dificulty++;
              //  mainBaseUI.SetActive(false);
                maxSteps--;
                x.GetComponentInChildren<PathFloor>().dificulty = dificulty + 1;
            } 
            else if (IsDivisible(maxSteps, 10))
            {
                if(maxSteps<30) SpawnCombat(EncounterType.Hard);
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
                    if (rando > 90-dificulty)
                    {
                        b = nextSteps[Random.Range(0, nextSteps.Length)];
                        islazer = true;
                    }

                    a = Instantiate(path, b.position, b.rotation, transform) as PathFloor;
                    a.maxSteps = maxSteps - 1;

                    laser.gameObject.SetActive(true);
                    laser2.gameObject.SetActive(true);
                    if (islazer)
                    {
                        laser.transform.LookAt(a.lasertarget);
                        laser2.transform.LookAt(a.lasertarget2);
                    }
                    else
                    {
                        laser.transform.LookAt(Vector3.zero);
                        laser2.transform.LookAt(Vector3.zero);
                    }
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
    private void SpawnCombat(EncounterType type)
    {
        PathCombat a = new PathCombat(); 


        switch (type)
        {
            case EncounterType.Easy:
                a = Instantiate(combats[Random.Range(0, combats.Length)], forward.position, forward.rotation, transform) as PathCombat;
                a.totalMobs = Random.Range(1, dificulty + 1);
                a.maxsteps = maxSteps - 1;
                a.dificulty = dificulty;
                a.StartCoroutine("Waiter");
                Debug.Log("ez");
                break;

            case EncounterType.Normal:
                a = Instantiate(combats[Random.Range(0, combats.Length)], forward.position, forward.rotation, transform) as PathCombat;
                var total = Random.Range(dificulty + 1, 2 * (dificulty + 1));
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
                a.StartCoroutine("Waiter");
                Debug.Log("Normal");
                break;

            case EncounterType.Hard:
                Debug.Log("Hard");
                a = SpawnNormal(ref forwardPlusUP);
                a.StartCoroutine("Waiter");
                a = SpawnNormal(ref leftPlusUP);
                a.StartCoroutine("Waiter");
                a = SpawnNormal(ref rightPlusUP);
                a.StartCoroutine("Waiter");
                break;

            case EncounterType.Impossible:

                break;
        }


    }

    private PathCombat SpawnNormal(ref Transform pos)
    {
        PathCombat a = Instantiate(combats[Random.Range(0, combats.Length)], pos.position, pos.rotation, transform) as PathCombat;
        var total = Random.Range(dificulty + 1, 2 * (dificulty + 1));
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
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("PathLevel", 0);
    }
}
