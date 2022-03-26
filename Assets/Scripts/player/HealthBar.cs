using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] TextUI hpText;

    private Image[] images;
    private void Awake()
    {
        images = GetComponentsInChildren<Image>(); ;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        hpText.UpdateText(""+health);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        hpText.UpdateText("" + health);
    }

    internal void ChangeToRed()
    {
        foreach (var current in images) current.color = Color.red;
    }

    internal void ChangeToGreen()
    {
        foreach (var current in images) current.color = Color.green;
    }
}
