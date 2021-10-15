using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsCrafting : MonoBehaviour
{
    public TMPro.TextMeshPro[] valuesInterior;
    [SerializeField] TMPro.TextMeshPro[] valuesExterior;
    [SerializeField] TMPro.TextMeshPro[] valuesSpecial;

    [SerializeField] TMPro.TextMeshPro[] tiers;

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
        if (gun != null)
        {
            foreach (Mod mod in gun.mods)
                    {
                        if (mod.grade.Equals(Grade.interior))
                        {
                            valuesInterior[i].text = mod.text;
                            tiers[i].text = "tier "+mod.tier.ToString();
                            mods[i].SetActive(true);
                            i++;
                        }else if (mod.grade.Equals(Grade.exterior))
                        {
                            valuesExterior[e].text = mod.text;
                            tiers[e+2].text = "tier " + mod.tier.ToString();
                            mods[e+2].SetActive(true);
                            e++;
                        }

                        else if (mod.grade.Equals(Grade.special))
                        {
                            valuesSpecial[s].text = mod.text;
                            tiers[s+4].text = "tier " + mod.tier.ToString();
                            mods[s+4].SetActive(true);
                            s++;
                        }

                    }
        }
        else
        {
            foreach(var x in mods)
            {
                x.SetActive(false);
            }
        }
        
    }

}
