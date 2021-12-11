using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDevice : MonoBehaviour
{
    public Material green;
    public Material blue;

    public AudioSource audioSource;
    public AudioClip[] sounds;

    public GameObject[] uiSlots;

    public TMPro.TextMeshPro[] names;

    private void Start()
    {
        if (sounds != null)
        {
            var i = 0;
            foreach (AudioClip sound in sounds)
            {
                

                if (uiSlots != null)
                {
                    var thisSlot = uiSlots[i];
                    thisSlot.SetActive(true);
                    TurnGreen(thisSlot.GetComponents<MeshRenderer>());
                    thisSlot.GetComponent<TMPro.TextMeshPro>().text = sound.name+"";
                }

                i++;
            }
        }

        //StartCoroutine(RotateText());
    }

    IEnumerator RotateText()
    {
        yield return new WaitForSeconds(0.8f);
        foreach(var name in names)
        {
           name.text = ShiftString(name.text);
        }
        StartCoroutine(RotateText());
    }

    public static string ShiftString(string t)
    {
        return t.Substring(1, t.Length - 1) + t.Substring(0, 1);
    }


    public void TurnGreen(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = green;
    }
    public void TurnBlue(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = blue;
    }

    internal void playSong(string songName, MeshRenderer[] materials)
    {
        foreach(var ui in uiSlots)
        {
            var x = ui.gameObject.GetComponentsInChildren<MeshRenderer>();
            TurnGreen(x);
        }

        foreach (AudioClip x in sounds) 
        {
            if (x.name == songName)
            {
                audioSource.clip = x;
                audioSource.Play();
                TurnBlue(materials);
            }
        }

    }
}
