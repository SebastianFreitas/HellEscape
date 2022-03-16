using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Room corridor1;
	[SerializeField] Room corridorLight;

	[SerializeField] Room wall;
	[SerializeField] Room stairs;

	[SerializeField] Room startRoomPrefab, endRoomPrefab;
	[SerializeField] List<Room> roomPrefabs = new List<Room>();
	[SerializeField] List<Room> SpecialRoomPrefabs = new List<Room>();


	List<Doorway> availableDoorways = new List<Doorway>();
	StartRoom startRoom;
	Room endRoom;
	internal List<Room> placedRooms = new List<Room>();
	internal Vector3 currentStartPos;

	Doorway nextDoorway;
	List<Doorway> otherDoorways = new List<Doorway>();

	void OnEnable()
	{

		StartCoroutine("GenerateLevel");
	}

	IEnumerator GenerateLevel()
    {

		WaitForSeconds startup = new WaitForSeconds(1);
		WaitForFixedUpdate interval = new WaitForFixedUpdate();

		yield return startup;

		// Place start room
		PlaceStartRoom();


		//place pathway
		yield return interval;
		for (int i = 1; i <=100; i++)
		{

			int a = 0;
            while (true)
            {
				a++;
				var rando = Random.Range(0, roomPrefabs.Count);

				if (i % 2 == 0)
				{
					PlaceCorridor(Random.Range(5, 10));
					break;
				}
				else if (PlaceRoom(roomPrefabs[rando], true)) break;

				if (a >= 10) break;
            }

		}

		//fill the rest of the level
		FillEmptyDoors();
		//finish the level
		PlaceCorridor(Random.Range(5, 10));
		PlaceEndRoom();
	}

    private void FillEmptyDoors()
    {
        foreach(Doorway door in otherDoorways)
        {
            while (true)
            {
				var current = SpecialRoomPrefabs[Random.Range(0, SpecialRoomPrefabs.Count)];

				// Instantiate room
				var room = Instantiate(current) as Room;
				room.transform.parent = this.transform;

				PositionRoomAtDoorway(ref room, room.doorways[0], door);

				if (CheckRoomOverlap(room))
				{
					var wallRoom = Instantiate(wall) as Room;
					PositionRoomAtDoorway(ref wallRoom, wallRoom.doorways[0], door);
					Destroy(room.gameObject);
					break;
				}
				else
                {
					placedRooms.Add(room);
					break;
                }
            }

		}
    }

    private void PlaceCorridor(int length)
    {
		if (Random.Range(0, 9) > 2) PlaceRoom(stairs, false);
		for (int i = 1; i <= length; i++)
		{
			if (i % 4 == 0) PlaceRoom(corridorLight, true);
			else PlaceRoom(corridor1, true);
		}
	}

    private void PlaceEndRoom()
    {
		// Instantiate room
		endRoom = Instantiate(endRoomPrefab) as Room;
		endRoom.transform.parent = this.transform;

		PositionRoomAtDoorway(ref endRoom, endRoom.doorways[0], nextDoorway);
	}


	void PlaceStartRoom()
	{
		// Instantiate room
		startRoom = Instantiate(startRoomPrefab) as StartRoom;
		startRoom.transform.parent = this.transform;

		nextDoorway = startRoom.doorways[0];
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

	bool PlaceRoom(Room room, bool shuffle)
    {
        // Instantiate room
        Room currentRoom = Instantiate(room) as Room;
        currentRoom.transform.parent = this.transform;

        if (shuffle && currentRoom.doorways.Length > 2) ShuffleDoors(currentRoom);


        int a = 0;
        while (true)
        {
            foreach (Doorway door in currentRoom.doorways)
            {
                PositionRoomAtDoorway(ref currentRoom, door, nextDoorway);
                if (!CheckRoomOverlap(currentRoom))
                {
					UpdateDoors(currentRoom, door);
					placedRooms.Add(currentRoom);
                    return true;
                }
            }

            a++;
            if (a >= 5)
            {
                Destroy(currentRoom.gameObject);
                return false;
            }
        }
    }

    private static void ShuffleDoors(Room currentRoom)
    {
        var alpha = currentRoom.doorways;
        for (int i = 0; i < alpha.Length; i++)
        {
            var temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Length);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    private void UpdateDoors(Room currentRoom, Doorway door)
    {
		bool nexDoorPicked = false;

		foreach (Doorway currentDoor in currentRoom.doorways)
		{
			if (!GameObject.ReferenceEquals(currentDoor.gameObject, door.gameObject))
			{
				if (nexDoorPicked) otherDoorways.Add(currentDoor);
				else
				{
					nextDoorway = currentDoor;
					nexDoorPicked = true;
				}
			}
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



