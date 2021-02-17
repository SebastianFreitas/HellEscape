using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{

    public PlayerMovement playerPrefab;
    List<Doorway> availableDoorways = new List<Doorway>();
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabs = new List<Room>();

    StartRoom startRoom;
    EndRoom endRoom;
    Room currentRoom;

    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
        currentRoom = Instantiate(startRoomPrefab);
        currentRoom.transform.parent = this.transform;
        
        InstantiatePlayerInRoom(startRoom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceRoomAndPlayer(Room room)
    {
        // Instantiate room
        currentRoom = Instantiate(room);
        currentRoom.transform.parent = this.transform;

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

        if (type == 1 || type == 2)
        {
            PlaceRoomAndPlayer(GenerateRoom());
        }
        else PlaceRoomAndPlayer(endRoom);


    }

    private void NextRoom(Room room)
    {

    }

    Room GenerateRoom()
    {
        Room returnRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        return returnRoom;
    }


}
