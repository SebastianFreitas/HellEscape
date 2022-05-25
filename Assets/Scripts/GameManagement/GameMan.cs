using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : ModDataRoom
{
    public string startScene;

    [Header("References")]
    public HealthBar hpBar;
    public GameObject player;

    public Transform runningGame;
    public Hub hub;
    public bool startAtHub;

    [SerializeField] internal RoomGenerator roomGen;
    [SerializeField] Vector3 currentStartPos;

    [SerializeField] PathFloor path;

    //[SerializeField] GameObject player;

    internal ModDataRoom.GeneratedMission mission;

    internal StartHub currentHub;

    internal bool runsucess = false;

    // Start is called before the first frame update
    void Start()
    {


        if (startAtHub)
        {
            hub.gameObject.SetActive(true);
            currentStartPos = hub.statspos.position;

            player.GetComponent<PlayerHpManager>().HealForMax();
            player.GetComponent<CharacterController>().enabled = false;

            player.transform.position = hub.statspos.position;
            player.transform.rotation = hub.statspos.rotation;

            player.GetComponent<CharacterController>().enabled = true;
        }
        else PlacePlayerInCurrentRoom();

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lava"),LayerMask.NameToLayer("Room"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BulletEnemy"), LayerMask.NameToLayer("Enemy"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BulletEnemy"), LayerMask.NameToLayer("BulletEnemy"));

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("OnlyEnemy"), false);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"), false);
    }
    internal bool startedRun = false;
    internal void ReturnToHub()
    {
        runsucess = false;
        currentHub.RefreshHub();
        roomGen.Clean();

        roomGen.ApplyMissionToPlayer(false);
        GoToHub();

    }

    private void GoToHub()
    {
        hub.gameObject.SetActive(true);

        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = hub.statspos.position;
        player.transform.rotation = hub.statspos.rotation;

        player.GetComponent<CharacterController>().enabled = true;


        currentStartPos = hub.statspos.position;

        player.GetComponent<PlayerHpManager>().HealForMax();
       
    }

    internal void EndRun()
    {
        runsucess = true;
        currentHub.RefreshHub();
        roomGen.Clean();

        roomGen.ApplyMissionToPlayer(false);
        GoToHub();
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
    }

    internal void VoidPlayer()
    {
        GoToHub();


    }

}
