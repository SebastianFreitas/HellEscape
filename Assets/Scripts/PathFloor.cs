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

    private void Start()
    {
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
              SpawnCombat();
            }
            else if (maxSteps > 0)
            {
                Transform b = forward;
                bool islazer = false;
                var rando = Random.Range(0, 100);
                if (rando > 75)
                {
                    b = nextSteps[Random.Range(0, nextSteps.Length)];
                    islazer = true;
                }
                var a = Instantiate(path, b.position, b.rotation, transform) as PathFloor;
                a.maxSteps = maxSteps - 1;
                laser.gameObject.SetActive(true);
                if (islazer)
                {
                    laser.transform.LookAt(a.lasertarget);
                    laser2.transform.LookAt(a.lasertarget2);
                }
                else
                {
                    laser.transform.LookAt(new Vector3(0, -1000, 0));
                    laser2.transform.LookAt(new Vector3(0,-1000,0));
                }

                //foreach (var current in a.additionalObjects)
                //{
                //    current.SetActive(false);
                //}
            }



        }




    }

    private void SpawnCombat()
    {
        PathCombat a = Instantiate(combats[Random.Range(0, combats.Length)], forward.position, forward.rotation, transform) as PathCombat;
        a.maxsteps = maxSteps - 1;
    }

    public bool IsDivisible(int x, int n)
    {
        return (x % n) == 0;
    }

    internal void ResetPath()
    {
        if (!victory)
        {
        passed = false;
        maxSteps = totalSteps;
        int i = 0;

        foreach (Transform child in this.transform)
        {
            if (i > 6)GameObject.Destroy(child.gameObject);
            i++;
        }
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
        PlayerPrefs.SetInt("PathLevel", dificulty);
    }
}
