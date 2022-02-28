using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : ModDataRoom
{
    
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabs = new List<Room>();

    StartRoom startRoom;
    EndRoom endRoom;
    public Room currentRoom;

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


    // Start is called before the first frame update
    void Start()
    {
        if (startAtHub)
        {
            GoToHub();

        }
        else
        {
            currentRoom = Instantiate(startRoomPrefab, runningGame);
            currentRoom.areaLevel = currentLevel;
            currentLevel++;
            // currentRoom.transform.parent = this.transform;


            PlacePlayerInCurrentRoom();

            currentRoom.player = player;
            currentRoom.PickLayout();
            currentRoom.SpawnObjects(dificulty);
        }


        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lava"),LayerMask.NameToLayer("Room"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BulletEnemy"), LayerMask.NameToLayer("Enemy"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }

    private void GoToHub()
    {
        hub.gameObject.SetActive(true);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = hub.statspos.position;
        player.GetComponent<CharacterController>().enabled = true;
    }

    void PlaceRoomAndPlayer(Room room)
    {
        // Instantiate room
        currentRoom = Instantiate(room, runningGame);
        currentRoom.areaLevel = currentLevel+mission.aditionalAreaLevel;

        currentRoom.mission = mission;
        currentRoom.transform.parent = runningGame;
        currentRoom.player = player;
        currentRoom.PickLayout();
        currentRoom.SpawnObjects(dificulty);

        PlacePlayerInCurrentRoom();

    }

    private void PlacePlayerInCurrentRoom()
    {
        //teleport player to new room
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = currentRoom.playerStart.position;
        player.GetComponent<CharacterController>().enabled = true;
    }

    private Room[] run;
    private int runProgress = 0;
    private GeneratedMission mission;
    public void StartRun(GeneratedMission mis)
    {
        currentLevel++;
        mission = mis;
        hub.gameObject.SetActive(false);
        runProgress = 0;
        run = new Room[30];
        int i = 0;
        for(; i < mission.distance; i++)
        {
            run[i] = GenerateRoom();
            run[i].areaLevel = currentLevel;
            run[i].mission = mission;
            run[i].player = player;
        }
        run[i] = null;

        PlaceRoomAndPlayer(run[runProgress]);
    }


    public void Next(int type)
    {
        Destroy(currentRoom.gameObject);
        dificulty++;

        runProgress++;
        if (run[runProgress] != null)
        {
            StartCoroutine(LoadingScreen());
        }
        else GoToHub();


    }

    public IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(1f);
        PlaceRoomAndPlayer(run[runProgress]);
    }

    public void UnlockDoor()
    {
        currentRoom.locked = true;
    }

    public void RestartGame()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        SceneManager.LoadScene(startScene);
    }

    Room GenerateRoom()
    {
        Room returnRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        return returnRoom;
    }

    Room CreateRoom()
    {

        return null;
    }
}
