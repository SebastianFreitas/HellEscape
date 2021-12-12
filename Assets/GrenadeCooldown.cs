using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeCooldown : MonoBehaviour
{

    public Slider slider;

    public float cdUI;
    public float maxCd;
    public void startCD(float cd)
    {
        cdUI = cd;
        SetMaxCD(cd);
        StartCoroutine(TimerDown());
    }

    public void startCDUP(float cd)
    {
        cdUI = 0;
        SetMaxCD(cd);
        StartCoroutine(TimerUp());
    }

    public void SetCD(float health)
    {
        slider.value = health;
        cdUI = health;
    }

    public void SetMaxCD(float health)
    {
        maxCd = health;
        slider.maxValue = health;
        slider.value = health;
    }
    private int counter = 0;
    IEnumerator TimerDown()
    {
        counter++;
        while (cdUI > 0 && counter ==1)
        {
            cdUI -= .5f;
            SetCD(cdUI);
            yield return new WaitForSecondsRealtime(.5f);

        }
        counter--;
    }

    private int counterUp = 0;
    IEnumerator TimerUp()
    {
        counterUp++;
        cdUI = 0;
        while (cdUI < maxCd && counterUp == 1)
        {
            cdUI += .5f;
            SetCD(cdUI);
            yield return new WaitForSecondsRealtime(.5f);

        }
        counterUp--;
    }

}
