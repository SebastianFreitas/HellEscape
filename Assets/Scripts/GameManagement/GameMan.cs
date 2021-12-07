using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : MonoBehaviour
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

    private int currentLevel = 50;

    public Transform runningGame;
    public Hub hub;
    public bool startAtHub;

    // Start is called before the first frame update
    void Start()
    {
        if (startAtHub)
        {
            hub.gameObject.SetActive(true);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = hub.statspos.position;
            player.transform.rotation = hub.statspos.rotation;
            player.GetComponent<CharacterController>().enabled = true;

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
            //currentRoom.SpawnObjects(dificulty);
        }


        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lava"),LayerMask.NameToLayer("Room"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
    }

    void PlaceRoomAndPlayer(Room room)
    {
        // Instantiate room
        currentRoom = Instantiate(room, runningGame);
        currentRoom.areaLevel = currentLevel;
        currentLevel++;
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
        //player.transform.rotation = currentRoom.playerStart.rotation;
        player.GetComponent<CharacterController>().enabled = true;
    }

    void InstantiatePlayerInRoom(Room room)
    {
        // Place Player
        //player = Instantiate(player, currentRoom.playerStart.position, currentRoom.playerStart.rotation) as GameObject;
        //player.transform.SetParent(runningGame);
       // player.GetComponent<PlayerHpManager>().hp = hpBar;
    }

    public void Next(int type)
    {
        Destroy(currentRoom.gameObject);
        dificulty++;

        

        if (type == 1 || type == 2)
        {
            StartCoroutine(LoadingScreen());//PlaceRoomAndPlayer(GenerateRoom());
        }
        else PlaceRoomAndPlayer(endRoom);


    }

    private IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(1f);
        PlaceRoomAndPlayer(GenerateRoom());
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


}
