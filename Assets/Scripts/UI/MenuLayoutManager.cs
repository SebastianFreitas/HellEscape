using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLayoutManager : MonoBehaviour
{
    [SerializeField] GameObject[] layouts;

    private int currenlayout;
    private GameObject layout;
    void Start()
    {
        currenlayout = Random.Range(0, layouts.Length);
        //layouts[currenlayout].SetActive(true);
        layout = Instantiate(layouts[currenlayout], transform);
        StartCoroutine(ChangeLayoutWaiter());
    }


    private IEnumerator ChangeLayoutWaiter()
    {
        yield return new WaitForSeconds(30);

        GenNewLayout();

        StartCoroutine(ChangeLayoutWaiter());
    }

    private void GenNewLayout()
    {
        var newLayout = Random.Range(0, layouts.Length);

        while (currenlayout == newLayout) newLayout = Random.Range(0, layouts.Length);

        Destroy(layout);
        currenlayout = newLayout;
        layout = Instantiate(layouts[currenlayout], transform);
    }
}
