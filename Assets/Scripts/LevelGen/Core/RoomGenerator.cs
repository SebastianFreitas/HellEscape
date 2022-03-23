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
	[SerializeField] List<Room> mainRooms = new List<Room>();
	[SerializeField] List<Room> sideRooms = new List<Room>();
	[SerializeField] List<Room> corridorPrefabs = new List<Room>();


	List<Doorway> availableDoorways = new List<Doorway>();
	StartRoom startRoom;
	Room endRoom;
	internal List<Room> placedRooms = new List<Room>();
	internal List<Room> placedWalls = new List<Room>();
	internal Vector3 currentStartPos;

	Doorway nextDoorway;
	List<Doorway> otherDoorways = new List<Doorway>();

	internal bool isGenerated = false;
	internal enum RoomType
	{
		SideRoom,
		MainRoom,
		EndRoom,
		StartRoom,
		Corridor
	}


	void OnEnable()
	{
		if (!isGenerated)
			StartCoroutine("GenerateLevel");
	}

	IEnumerator GenerateLevel()
    {

		WaitForSeconds startup = new WaitForSeconds(.1f);
		WaitForFixedUpdate interval = new WaitForFixedUpdate();

		yield return startup;

		// Place start room
		PlaceStartRoom();


		//place pathway
		for (int i = 1; i <=50; i++)
		{
			yield return startup;
			int a = 0;
            while (true)
            {
				a++;
				var rando = Random.Range(0, mainRooms.Count);

				var x = Random.Range(1, 4);
                bool worked;
                if (x >= 2) worked = PlaceCorridor(Random.Range(5, 10));

				else worked = PlaceRoom(mainRooms[rando], true, RoomType.MainRoom);

				if (worked) break;

				if (a >= 10) ResetLevelGenerator();

			}

		}

		//fill the rest of the level
		FillEmptyDoors();
		yield return startup;
		//finish the level
		PlaceCorridor(Random.Range(5, 10));
		yield return startup;
		if (!PlaceEndRoom()) ResetLevelGenerator(); 


		isGenerated = true;
	}

    private void FillEmptyDoors()
    {
        foreach(Doorway door in otherDoorways)
        {
			var x = Random.Range(1, 3);

			if (x == 2)
			{
				var wallRoom = Instantiate(wall) as Room;
				wallRoom.transform.parent = this.transform;
				placedWalls.Add(wallRoom);

				PositionRoomAtDoorway(ref wallRoom, wallRoom.doorways[0], door);
			}
			else
            {
				while (true)
				{
					var current = sideRooms[Random.Range(0, sideRooms.Count)];

					// Instantiate room
					var room = Instantiate(current) as Room;
					room.transform.parent = this.transform;

					PositionRoomAtDoorway(ref room, room.doorways[0], door);

					if (CheckRoomOverlap(room))
					{
						var wallRoom = Instantiate(wall) as Room;
						wallRoom.transform.parent = this.transform;
						placedWalls.Add(wallRoom);

						PositionRoomAtDoorway(ref wallRoom, wallRoom.doorways[0], door);

						Destroy(room.gameObject);
						break;
					}
					else
					{
						placedRooms.Add(room);

						var y = Random.Range(1,21);
						if (y >15) room.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Special;
						else room.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Encounter;
						
						break;
					}
				}
            }

		}
    }

    private bool PlaceCorridor(int length)
    {
		PlaceRoom(stairs, false, RoomType.Corridor);
		return PlaceRoom(corridorPrefabs[Random.Range(0, corridorPrefabs.Count)], true, RoomType.Corridor);

		//     for (int i = 1; i <= length; i++)
		//     {
		//var counter = 0;
		//while (true)
		//         {

		//             var rando = Random.Range(0, 3);
		//             var worked = false;
		//             switch (rando)
		//             {
		//                 case 0:
		//                     worked = PlaceRoom(corridorLight, true);
		//                     break;

		//                 case 1:
		//                     worked = PlaceRoom(stairs, false);
		//                     break;

		//                 case 2:
		//                     worked = PlaceRoom(corridorPrefabs[Random.Range(0, corridorPrefabs.Count)], true);
		//                     break;
		//             }

		//             if (worked) break;
		//             else counter++;

		//             if (counter > 5) return false;
		//         }
		//     }
	}

    private bool PlaceEndRoom()
    {
		// Instantiate room
		endRoom = Instantiate(endRoomPrefab) as Room;
		endRoom.transform.parent = this.transform;

		PositionRoomAtDoorway(ref endRoom, endRoom.doorways[Random.Range(0,endRoom.doorways.Length)], nextDoorway);
		if (!CheckRoomOverlap(endRoom)) return true;
		else return false;
	}

	void PlaceStartRoom()
	{
		// Instantiate room
		startRoom = Instantiate(startRoomPrefab) as StartRoom;
		startRoom.transform.parent = this.transform;

		nextDoorway = startRoom.doorways[0];
		// Position room
		startRoom.transform.localPosition = Vector3.zero;
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

	bool PlaceRoom(Room room, bool shuffle, RoomType type)
    {
        // Instantiate room
        Room currentRoom = Instantiate(room) as Room;
        currentRoom.transform.parent = this.transform;

        if (shuffle && currentRoom.doorways.Length >= 2) ShuffleDoors(currentRoom);

        while (true)
        {
            foreach (Doorway door in currentRoom.doorways)
            {
                PositionRoomAtDoorway(ref currentRoom, door, nextDoorway);
				if (!CheckRoomOverlap(currentRoom))
				{
					UpdateDoors(currentRoom, door);
					placedRooms.Add(currentRoom);
					if (type == RoomType.MainRoom)currentRoom.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Encounter;
					return true;
				}
			}

            Destroy(currentRoom.gameObject);
            return false;

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
				continue;
			}
			else if (bounds.Intersects(currentRoom.roomBounds))
            {
                return true;
            }
        }


		return false;
	}

	void ResetLevelGenerator()
    {
        Debug.LogError("Reset level generator");

        StopCoroutine("GenerateLevel");

		// Delete all rooms
		if (startRoom) Destroy(startRoom.gameObject);
        if (endRoom) Destroy(endRoom.gameObject);


        foreach (Room room in placedRooms) Destroy(room.gameObject);
		foreach (var wall in placedWalls) Destroy(wall.gameObject);

        // Clear lists
        placedRooms.Clear();
        availableDoorways.Clear();
		nextDoorway = null;
		otherDoorways.Clear();
		placedWalls.Clear();

        // Reset coroutine
        StartCoroutine("GenerateLevel");
    }
}



