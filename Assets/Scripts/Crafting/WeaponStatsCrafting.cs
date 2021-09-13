using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsCrafting : MonoBehaviour
{
    public TMPro.TextMeshPro[] valuesInterior;
    [SerializeField] TMPro.TextMeshPro[] valuesExterior;
    [SerializeField] TMPro.TextMeshPro[] valuesSpecial;

    public GameObject[] mods;



    public GunOfAType gun;
    void OnEnable()
    {
        UpdateUI();
    }


    public void ResetUI()
    {
        foreach (GameObject x in mods) x.SetActive(false);
    }

    public void UpdateUI()
    {
        var i = 0;
        var e = 0;
        var s = 0;
        foreach (Mod mod in gun.mods)
        {
            if (mod.grade.Equals(Grade.interior))
            {
                valuesInterior[i].text = mod.text;
                mods[i].SetActive(true);
                i++;
            }

            if (mod.grade.Equals(Grade.exterior))
            {
                valuesExterior[e].text = mod.text;
                mods[e+2].SetActive(true);
                e++;
            }

            if (mod.grade.Equals(Grade.special))
            {
                valuesSpecial[s].text = mod.text;
                mods[s+4].SetActive(true);
                s++;
            }

        }
    }

}
