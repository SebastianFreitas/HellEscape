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
    private void Start()
    {
        roomActivator = GetComponentInParent<RoomActivator>();


        influenceEffect = new InfluenceEffect(roomActivator.influcence);

        CreateText();
    }

    private void CreateText()
    {

        switch (influenceEffect.influenceType)
        {
            case RoomActivator.AreaType.Blue:
                redName.text = "Pursue" + redNames[Random.Range(0, redNames.Length)];
                blueName.text = "Preserve" + blueNames[Random.Range(0, blueNames.Length)];
                break;

            case RoomActivator.AreaType.Red:
                redName.text = "Preserve" + redNames[Random.Range(0, redNames.Length)];
                blueName.text = "Pursue" + blueNames[Random.Range(0, blueNames.Length)];
                break;
        }
        
        blueEffectText.text += "<color=red>" + influenceEffect.blueBadText + "</color>\n";
        blueEffectText.text += "<color=green>" + influenceEffect.blueGoodText + "</color>\n";
        redEffectText.text += "<color=red>" + influenceEffect.redBadText + "</color>\n";
        redEffectText.text += "<color=green>" + influenceEffect.redGoodText + "</color>\n";
    }

    InfluenceEffect influenceEffect;


    internal void TurnBlue()
    {

        influenceEffect.PickBlue(roomActivator.GetComponentInParent<RoomGenerator>());

        FindObjectOfType<RoomGenerator>().ChangeInfluence(RoomActivator.AreaType.Blue);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"));
        DisableEvent();
    }

    internal void TurnRed()
    {
        influenceEffect.PickRed(roomActivator.GetComponentInParent<RoomGenerator>());
        FindObjectOfType<RoomGenerator>().ChangeInfluence(RoomActivator.AreaType.Red);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"), false);
        DisableEvent();
    }
    internal void DisableEvent()
    {
        blue.SetActive(false);
        red.SetActive(false);
    }


}
