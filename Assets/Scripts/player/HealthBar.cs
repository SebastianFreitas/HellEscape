using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] TextUI hpText;
    //[SerializeField] TMPro.TextMeshPro text;
    private Image[] images;
    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
        UIUpdate();

    }
    private int life=50;
    private int maxLife = 50;
    private void UIUpdate()
    {
        hpText.UpdateText( $"[{life}/{maxLife}]");
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        life = health;
        UIUpdate();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        maxLife = health;
        UIUpdate();
        //slider.value = health;
        //hpText.UpdateText("" + health);
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
