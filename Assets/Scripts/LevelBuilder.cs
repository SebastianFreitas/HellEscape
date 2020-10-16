using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public Room startRoomPrefab, endRoomPrefab;
    public List<Room> roomPrefabs = new List<Room>();
    public Vector2 iterationRange = new Vector2 (3,10);

    List<Doorway> availableDoorways = new List <Doorway>();

    StartRoom startRoom;
    EndRoom endRoom;
    List<Room> placedRooms = new List<Room> ();

    LayerMask roomLayerMask;

    void Start()
    {
      roomLayerMask = LayerMask.GetMask("Room");
      StartCoroutine ("GenerateLevel");
    }

    IEnumerator GenerateLevel()
    {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate ();

        yield return startup;

        //place start room
        Debug.Log("place start room");
        PlaceStartRoom();
        yield return interval;

        //random iterations
        int iterations = Random.Range((int)iterationRange.x,(int)iterationRange.y);

        for(int i = 0; i < iterations; i++)
        {
          //place random room from List
          Debug.Log("Place random room from list");
          PlaceRoom();
          yield return interval;
        }

        //place end room
        Debug.Log("place end room");
        PlaceEndRoom();
        yield return interval;

        //level generation finished
        Debug.Log("level generation finished");
        yield return new WaitForSeconds(3);
        ResetLevelGenerator();

    }

    void PlaceStartRoom()
    {
      //instantiate room
      startRoom = Instantiate (startRoomPrefab) as StartRoom;
      startRoom.transform.parent = this.transform;

      //get doorways from current room and add them randomly to the list of available availableDoorways
      AddDoorwaysToList (startRoom, ref availableDoorways);

      //position room
      startRoom.transform.position = Vector3.zero;
      startRoom.transform.rotation = Quaternion.identity;

    }

    void AddDoorwaysToList(Room room, ref List<Doorway> list)
    {
        foreach (Doorway doorway in room.doorways){
          int r = Random.Range (0, list.Count);
          list.Insert(r,doorway);
        }
    }

    void PlaceRoom()
    {
      // Instantiate room
      Room currentRoom = Instantiate (roomPrefabs[Random.Range(0, roomPrefabs.Count)]) as Room;
      currentRoom.transform.parent = this.transform;

      //create doorway lists to loop override
      List<Doorway> allAvailableDoorways = new List<Doorway> (availableDoorways);
    }

    void PlaceEndRoom()
    {

    }

    void ResetLevelGenerator ()
    {
      Debug.LogError("Reset level generator");

      StopCoroutine ("GenerateLevel");

      //delete all room
      if(startRoom){
         Destroy (startRoom.gameObject);
      }

      if (endRoom){
        Destroy (endRoom.gameObject);
      }

      foreach(Room room in placedRooms){
        Destroy (room.gameObject);
      }

      //clear Lists
      placedRooms.Clear();
      availableDoorways.Clear();
      //reset Coroutine
      StartCoroutine ("GenerateLevel");
    }

}
