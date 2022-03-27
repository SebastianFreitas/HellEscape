using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceRoom : MonoBehaviour
{
    [SerializeField] GameObject blue;
    [SerializeField] GameObject red;

    [SerializeField] TMPro.TextMeshPro blueName;
    [SerializeField] TMPro.TextMeshPro redName;


    [SerializeField] TMPro.TextMeshPro blueEffectText;
    [SerializeField] TMPro.TextMeshPro redEffectText;


    private string[] blueNames = { "[E643-SSA-Code:Alvin Murphy]" };
    private string[] redNames = { "[E931-ASS-Code:John Dillard]" };
    RoomActivator roomActivator;
    private void Awake()
    {
        roomActivator = GetComponentInParent<RoomActivator>();

        blueName.text = blueNames[Random.Range(0, blueNames.Length)];
        redName.text = redNames[Random.Range(0, redNames.Length)];


        switch (roomActivator.influcence)
        {
            case RoomActivator.AreaType.Blue:
                ContinueBlueText();
                ChangeToRedText();
                break;
            case RoomActivator.AreaType.Red:
                ContinueRedText();
                ChangeToBlueText();
                break;
        }
    }
    BlueEffect blueEffect;
    internal void ContinueBlueText()
    {
        blueName.text = "Preserve"+ blueNames[Random.Range(0, blueNames.Length)];

        blueEffect = BlueEffect.GetNewBlueEffect(BlueEffect.ContactType.Preserve);
    }

    internal void ChangeToBlueText()
    {
        blueName.text = "Pursue" + blueNames[Random.Range(0, blueNames.Length)];
    }

    internal void ContinueRedText()
    {
        redName.text = "Preserve" + redNames[Random.Range(0, redNames.Length)];
    }

    internal void ChangeToRedText()
    {
        redName.text = "Pursue" + redNames[Random.Range(0, redNames.Length)];
    }

    internal void TurnBlue()
    {

    }

    internal void TurnRed()
    {

    }
    internal void DisableEvent()
    {
        blue.SetActive(false);
        red.SetActive(false);
    }
    // Update is called once per frame

}
