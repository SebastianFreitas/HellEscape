using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Room corridor1;
	[SerializeField] Room corridorLight;

	[SerializeField] Room startRoomPrefab, endRoomPrefab;
	[SerializeField] List<Room> roomPrefabs = new List<Room>();
	[SerializeField] Vector2 iterationRange = new Vector2(3, 10);


	List<Doorway> availableDoorways = new List<Doorway>();
	StartRoom startRoom;
	EndRoom endRoom;
	internal List<Room> placedRooms = new List<Room>();
	internal Vector3 currentStartPos;


	LayerMask roomLayerMask;
	void OnEnable()
	{
		roomLayerMask = LayerMask.GetMask("Room");
		StartCoroutine("GenerateLevel");
	}
	IEnumerator GenerateLevel()
    {

		WaitForSeconds startup = new WaitForSeconds(1);
		WaitForFixedUpdate interval = new WaitForFixedUpdate();

		yield return startup;

		// Place start room
		PlaceStartRoom();

		yield return interval;
		for (int i = 1; i <= 100; i++)
		{
            //if (i % 2 == 0) PlaceCorridor(Random.Range(5, 5));
            //else PlaceRoom(roomPrefabs[Random.Range(0, roomPrefabs.Count)]);

            PlaceRoom(roomPrefabs[Random.Range(0, roomPrefabs.Count)]);
		}
	}

    private void PlaceCorridor(int length)
    {
        for(int i = 1; i <= length; i++)
        {
			if (i % 4 == 0 ) PlaceRoom(corridorLight);
			else PlaceRoom(corridor1);
        }
    }

    void PlaceStartRoom()
	{
		// Instantiate room
		startRoom = Instantiate(startRoomPrefab) as StartRoom;
		startRoom.transform.parent = this.transform;

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList(startRoom, ref availableDoorways);

		// Position room
		startRoom.transform.position = Vector3.zero;
		startRoom.transform.rotation = Quaternion.identity;

		currentStartPos = startRoom.playerStart.position;
	}

	void PositionRoomAtDoorway(ref Room room, Doorway roomDoorway, Doorway targetDoorway)
	{
		// Reset room position and rotation
		room.transform.position = Vector3.zero;
		room.transform.rotation = Quaternion.identity;

		// Rotate room to match previous doorway orientation
		Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
		Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
		float deltaAngle = Mathf.DeltaAngle(roomDoorwayEuler.y, targetDoorwayEuler.y);
		Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
		room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);

		// Position room
		Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
		room.transform.position = targetDoorway.transform.position - roomPositionOffset;
	}
	void PlaceRoom(Room room)
	{
		// Instantiate room
		Room currentRoom = Instantiate(room) as Room;
		currentRoom.transform.parent = this.transform;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
		List<Doorway> currentRoomDoorways = new List<Doorway>();
		AddDoorwaysToList(currentRoom, ref currentRoomDoorways);

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList(currentRoom, ref availableDoorways);

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways)
		{
			// Try all available doorways in current room
			foreach (Doorway currentDoorway in currentRoomDoorways)
			{
				// Position room
				PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);

				// Check room overlaps
				if (CheckRoomOverlap(currentRoom))
				{
					continue;
				} 

				roomPlaced = true;

				// Add room to list
				placedRooms.Add(currentRoom);

				// Remove occupied doorways
				currentDoorway.gameObject.SetActive(false);
				availableDoorways.Remove(currentDoorway);

				availableDoorway.gameObject.SetActive(false);
				availableDoorways.Remove(availableDoorway);

				// Exit loop if room has been placed
				break;
			}

			// Exit loop if room has been placed
			if (roomPlaced)
			{
				break;
			}
		}

		// Room couldn't be placed. Restart generator and try again
		if (!roomPlaced)
		{
			Destroy(currentRoom.gameObject);
			ResetLevelGenerator();
		} 


	}
	void AddDoorwaysToList(Room room, ref List<Doorway> list)
	{
		foreach (Doorway doorway in room.doorways)
		{
			int r = Random.Range(0, list.Count);
			list.Insert(r, doorway);
		}
	}

	bool CheckRoomOverlap(Room room)
	{
		room.GetBounds();
		Bounds bounds = room.roomBounds;
		bounds.Expand(-0.1f);
		bounds.size /= 1.5f;


		foreach (Room currentRoom in placedRooms)
        {
			if (GameObject.ReferenceEquals(currentRoom.gameObject, room.gameObject))
			{
				Debug.Log("its the same room");
				continue;
			}
			else if (bounds.Intersects(currentRoom.roomBounds))
            {
                Debug.LogError("Overlap detected");
                return true;
            }
        }

		//Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size , room.transform.rotation);
		//if (colliders.Length > 0)
		//{
		//	// Ignore collisions with current room
		//	foreach (Collider c in colliders)
		//	{
		//		if (c.transform.parent.gameObject.Equals(room.gameObject))
		//		{
		//			continue;
		//		}
		//		else
		//		{
		//			Debug.LogError("Overlap detected");
		//			return true;
		//		}
		//	}
		//}

		return false;
	}

	void ResetLevelGenerator()
    {
        //Debug.LogError("Reset level generator");

        StopCoroutine("GenerateLevel");

        // Delete all rooms
        if (startRoom)
        {
            Destroy(startRoom.gameObject);
        }

        if (endRoom)
        {
            Destroy(endRoom.gameObject);
        }

        foreach (Room room in placedRooms)
        {
            Destroy(room.gameObject);
        }

        // Clear lists
        placedRooms.Clear();
        availableDoorways.Clear();

        // Reset coroutine
        StartCoroutine("GenerateLevel");
    }


}

