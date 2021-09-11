using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsCrafting : MonoBehaviour
{
    public TMPro.TextMeshPro[] valuesInterior;
    [SerializeField] TMPro.TextMeshPro[] valuesExterior;
    [SerializeField] TMPro.TextMeshPro[] valuesSpecial;


    [SerializeField] TMPro.TextMeshPro[] typesInterior;
    [SerializeField] TMPro.TextMeshPro[] typesExterior;
    [SerializeField] TMPro.TextMeshPro[] typesSpecial;

    public GunOfAType gun;
    void OnEnable()
    {
        UpdateUI();
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
                typesInterior[i].text = mod.grade.ToString();
                i++;
            }

            if (mod.grade.Equals(Grade.exterior))
            {
                valuesExterior[e].text = mod.text;
                typesExterior[e].text = mod.grade.ToString();
                e++;
            }

            if (mod.grade.Equals(Grade.special))
            {
                valuesSpecial[s].text = mod.text;
                typesSpecial[s].text = mod.grade.ToString();
                s++;
            }

        }
    }

}
