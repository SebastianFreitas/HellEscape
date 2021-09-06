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

    private GameObject player;
    private int dificulty = 1;

    public string startScene;

    [Header("References")]
    public HealthBar hpBar;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        currentRoom = Instantiate(startRoomPrefab);
        currentRoom.transform.parent = this.transform;
        
        
        InstantiatePlayerInRoom(startRoom);
        currentRoom.player = player;
        currentRoom.SpawnEnemies(1);
        currentRoom.SpawnObjects(dificulty);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lava"),LayerMask.NameToLayer("Room"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
    }

    void PlaceRoomAndPlayer(Room room)
    {
        // Instantiate room
        currentRoom = Instantiate(room);
        currentRoom.transform.Rotate(0, 0, Random.Range(-180, 180));
        currentRoom.player = player;
        currentRoom.SpawnEnemies(1);
        currentRoom.SpawnObjects(dificulty);

        //teleport player to new room
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = currentRoom.playerStart.position;
        player.transform.rotation = currentRoom.playerStart.rotation;
        player.GetComponent<CharacterController>().enabled = true;

    }

    void InstantiatePlayerInRoom(Room room)
    {
        // Place Player
        player = Instantiate(playerPrefab, currentRoom.playerStart.position, currentRoom.playerStart.rotation) as GameObject;
        player.transform.SetParent(transform);
        player.GetComponent<PlayerHpManager>().hp = hpBar;
    }

    public void Next(int type)
    {
        Destroy(currentRoom.gameObject);
        dificulty++;

        

        if (type == 1 || type == 2)
        {
            PlaceRoomAndPlayer(GenerateRoom());
        }
        else PlaceRoomAndPlayer(endRoom);


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
