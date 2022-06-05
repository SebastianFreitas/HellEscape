using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    public Camera fpsCam;
    private RaycastHit hit;
    
    public AudioClip teleport;
    public AudioClip wrong;
    public AudioClip click;

    private float volume = 1f;

    private GameMan gameMan;

    void OnEnable()
    {
        volume = PlayerPrefs.GetFloat("Volume");
        gameMan = transform.root.GetComponent<GameMan>();
        
    }

    void Start()
    {
        //GetComponent<CharacterController>().gameObject.layer = LayerMask.NameToLayer("Player");
        rayMask = 1 << LayerMask.NameToLayer("Player");
        rayMask = ~rayMask;
    }


    LayerMask rayMask;

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit, 10f, rayMask, QueryTriggerInteraction.Collide) )
            {

                if (hit.transform.CompareTag("Item")) hit.collider.transform.GetComponent<Item>().CollectItem();
                else if (hit.transform.CompareTag("ItemLife"))
                {
                    hit.collider.transform.GetComponent<HealthPack>().CollectItem();
                }
                else if (hit.transform.CompareTag("Button"))
                {
                    hit.collider.transform.GetComponent<SelectGun>().Select();
                }
                else if (hit.transform.CompareTag("AddMod"))
                {
                    hit.collider.transform.GetComponent<AddModUI>().Function();
                }
                else if (hit.transform.CompareTag("RemoveMod"))
                {
                    hit.collider.transform.GetComponent<RemoveMod>().Function();
                }
                else if (hit.transform.CompareTag("RemoveAndSaveMod"))
                {
                    hit.collider.transform.GetComponent<RemoveAndSave>().Function();
                }
                else if (hit.transform.CompareTag("DestroyGun"))
                {
                    hit.collider.transform.GetComponent<Destroy>().Function();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("Deconstruct"))
                {
                    hit.collider.transform.GetComponent<DisassembleGun>().Function();
                }
                else if (hit.transform.CompareTag("Generate"))
                {
                    hit.collider.transform.GetComponent<Generate>().Function();
                }
                else if (hit.transform.CompareTag("Exit"))
                {
                    hit.collider.transform.GetComponent<ExitCrafting>().Function();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("CraftingDevice"))
                {
                    hit.collider.transform.parent.GetComponentInParent<CraftingDevice>().TurnOn();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("SelectMusic"))
                {
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                    var songName = hit.collider.gameObject.GetComponentInParent<TMPro.TextMeshPro>().text;
                    MeshRenderer[] meshes = hit.collider.gameObject.GetComponentsInChildren<MeshRenderer>();

                    hit.collider.transform.parent.GetComponentInParent<SoundDevice>().playSong(songName, meshes);
                }
                else if (hit.transform.CompareTag("PauseMusic"))
                {
                    hit.collider.transform.parent.GetComponentInParent<SoundDevice>().PauseSong();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("StartMission"))
                {
                    hit.collider.transform.GetComponentInParent<MissionSelector>().OpenPortal();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("SelectMission"))
                {
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                    var x = hit.collider.transform.GetComponentInParent<MonitorMission>();
                    x.UISelect();
                    var mi = x.mission;

                    var selector = hit.collider.transform.parent.GetComponentInParent<MissionSelector>();
                    selector.currentMission = mi;
                    selector.UIUnselect(mi);
                }
                else if (hit.transform.CompareTag("ReloadMissions"))
                {

                    hit.collider.transform.parent.GetComponentInParent<MissionSelector>().ReloadMissions();

                }
                else if (hit.transform.CompareTag("SearchPath"))
                {

                    StartCoroutine(hit.collider.transform.parent.GetComponentInParent<MissionSelector>().SearchPath());

                }
                else if (hit.transform.CompareTag("Symbol"))
                {
                    hit.collider.transform.GetComponentInParent<SpecialRoom>().OpenDoor();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("SpawnMonster"))
                {
                    hit.collider.transform.GetComponentInParent<MonsterSpawber>().SpawnMonster();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("Error"))
                {
                    hit.collider.transform.GetComponentInParent<MonsterSpawber>().Error();
                }
                else if (hit.transform.CompareTag("VoidBoon"))
                {
                    hit.collider.transform.GetComponentInParent<ItemRoom>().GenerateBoon();
                }
                else if (hit.transform.CompareTag("WeaponDrop"))
                {
                    hit.collider.transform.GetComponentInParent<ItemRoom>().DropWeapon();
                }
                else if (hit.transform.CompareTag("HealDrop"))
                {
                    var x = hit.collider.transform.GetComponentInParent<ItemRoom>();
                    if (x) x.DropHeal();
                    else
                    {
                        var y = hit.collider.transform.GetComponentInParent<ChooseSpecial>();
                        y.DropHeal();
                    }

                }
                else if (hit.transform.CompareTag("YesVoid"))
                {
                    hit.collider.transform.GetComponentInParent<BoonDrop>().Accept();
                }
                else if (hit.transform.CompareTag("DenyVoid"))
                {
                    hit.collider.transform.GetComponentInParent<BoonDrop>().Deny();
                }
                else if (hit.transform.CompareTag("Blue"))
                {
                    hit.collider.transform.GetComponentInParent<InfluenceRoom>().TurnBlue();
                }
                else if (hit.transform.CompareTag("Red"))
                {
                    hit.collider.transform.GetComponentInParent<InfluenceRoom>().TurnRed();
                }
                else if (hit.transform.CompareTag("SpawnBench"))
                {
                    hit.collider.transform.GetComponentInParent<ChooseSpecial>().SpawnBench();
                }
                else if (hit.transform.CompareTag("NormalEncounter"))
                {
                    hit.collider.transform.GetComponentInParent<ChooseSpecial>().NormalEncounter();
                }
                else if (hit.transform.CompareTag("HardEncounter"))
                {
                    hit.collider.transform.GetComponentInParent<ChooseSpecial>().HardEncountner();
                }
                else if (hit.transform.CompareTag("ExtremeEncounter"))
                {
                    hit.collider.transform.GetComponentInParent<ChooseSpecial>().ExtremeEncounter();
                }
                else if (hit.transform.CompareTag("ShopBuy"))
                {
                    hit.collider.transform.GetComponentInParent<Shop>().Buy();
                }
                else if (hit.transform.CompareTag("ShopNext"))
                {
                    hit.collider.transform.GetComponentInParent<Shop>().PressNext();
                }
                else if (hit.transform.CompareTag("Mirror"))
                {
                    if (hit.collider.transform.GetComponent<MirrorButtons>().PushButton())
                    {
                        AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                    }
                    else AudioSource.PlayClipAtPoint(wrong, transform.position, .3f);
                }
                else if (hit.transform.CompareTag("Message"))
                {
                    hit.collider.gameObject.SetActive(false);
                    hit.collider.transform.GetComponentInParent<StartHub>().Activate();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else if (hit.transform.CompareTag("door"))
                {
                    if (!gameMan.isInEncounter)
                    {
                        hit.collider.gameObject.SetActive(false);
                        AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                    }
                    else AudioSource.PlayClipAtPoint(wrong, transform.position, .1f);

                }
                else if (hit.transform.CompareTag("Return"))
                {
                    transform.root.GetComponent<GameMan>().EndRun();
                    AudioSource.PlayClipAtPoint(click, transform.position, .1f);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(wrong, transform.position, .3f);
                }
            
        }
            else AudioSource.PlayClipAtPoint(wrong, transform.position, .3f);

        }
    }




}
