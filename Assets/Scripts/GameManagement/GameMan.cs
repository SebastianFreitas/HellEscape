using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : ModDataRoom
{

    //private GameObject player;
    private int dificulty = 1;

    public string startScene;

    [Header("References")]
    public HealthBar hpBar;
    public GameObject player;

    private int currentLevel = 90;

    public Transform runningGame;
    public Hub hub;
    public bool startAtHub;

    [SerializeField] RoomGenerator roomGen;
    [SerializeField] Vector3 currentStartPos;

    [SerializeField] PathFloor path;

    internal ModDataRoom.GeneratedMission mission;

    // Start is called before the first frame update
    void Start()
    {
        if (startAtHub)
        {
            GoToHub();

        }
        else
        {

            // currentRoom.transform.parent = this.transform;


            PlacePlayerInCurrentRoom();


        }


        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lava"),LayerMask.NameToLayer("Room"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BulletEnemy"), LayerMask.NameToLayer("Enemy"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"), false);
    }

    internal void ReturnToHub()
    {
        roomGen.Clean();

        roomGen.ApplyMissionToPlayer(false);

        GoToHub();

    }

    internal void PreloadRun()
    {
        roomGen.gameObject.SetActive(true);
        roomGen.gameObject.transform.position = roomGen.gameObject.transform.position + new Vector3(0, -10000, 0);
    }

  
    private void GoToHub()
    {
        hub.gameObject.SetActive(true);

        //player.GetComponentInChildren<MouseLook>().enabled = false;
        //player.GetComponent<PlayerBasicMovement>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = hub.statspos.position;
        player.transform.rotation = hub.statspos.rotation;

        player.GetComponent<CharacterController>().enabled = true;
        //player.GetComponentInChildren<MouseLook>().enabled = true;
        //player.GetComponent<PlayerBasicMovement>().enabled = true;

        currentStartPos = hub.statspos.position;

        player.GetComponent<PlayerHpManager>().HealForMax();
        hub.ResetPath();
        //path.ResetPath();
    }


    private void PlacePlayerInCurrentRoom()
    {
        //teleport player to new room
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = roomGen.currentStartPos;

        player.GetComponent<CharacterController>().enabled = true;

        currentStartPos = roomGen.currentStartPos;
    }



    public void StartRun(GeneratedMission mis)
    {

        mission = mis;
        hub.gameObject.SetActive(false);

        roomGen.StartRun(mis);

        ApplyMissionToPlayer(mis);
        
    }

    private void ApplyMissionToPlayer(GeneratedMission mis)
    {

    }

    public IEnumerator LoadingScreen()
    {
        player.gameObject.SetActive(false);
        while (true)
        {
            yield return new WaitForSecondsRealtime(3f);
            if (roomGen.isGenerated)
            {
                player.gameObject.SetActive(true);
                PlacePlayerInCurrentRoom();
                break;
            }
        }

    }


    internal void VoidPlayer()
    {
        //player.GetComponent<CharacterController>().enabled = false;
        //player.transform.position = currentStartPos;
        //player.GetComponent<CharacterController>().enabled = true;

        GoToHub();

    }

}
