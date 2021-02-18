using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : MonoBehaviour
{

    public PlayerMovement playerPrefab;
    List<Doorway> availableDoorways = new List<Doorway>();
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabs = new List<Room>();

    StartRoom startRoom;
    EndRoom endRoom;
    public Room currentRoom;

    public PlayerMovement player;
    private int dificulty = 1;

    public string startScene;

    // Start is called before the first frame update
    void Start()
    {
        
        currentRoom = Instantiate(startRoomPrefab);
        currentRoom.transform.parent = this.transform;
        
        
        InstantiatePlayerInRoom(startRoom);
        currentRoom.SpawnEnemies(dificulty);
    }

    void PlaceRoomAndPlayer(Room room)
    {
        // Instantiate room
        currentRoom = Instantiate(room);
        currentRoom.transform.parent = this.transform;
        currentRoom.SpawnEnemies(dificulty);

        //teleport player to new room
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = currentRoom.playerStart.position;
        player.transform.rotation = currentRoom.playerStart.rotation;
        player.GetComponent<CharacterController>().enabled = true;

        // Get doorways from current room and add them randomly to the list of available doorways
        //AddDoorwaysToList

        // Position room
        //startRoom.transform.position = Vector3.zero;
        //startRoom.transform.rotation = Quaternion.identity;
    }

    void InstantiatePlayerInRoom(Room room)
    {
        // Place Player
        player = Instantiate(playerPrefab, currentRoom.playerStart.position, currentRoom.playerStart.rotation) as PlayerMovement;
        player.transform.SetParent(transform);
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
