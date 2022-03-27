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
	internal List<Room> placedSideRooms = new List<Room>();
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


    //void OnEnable()
    //{
    //	if (!isGenerated)
    //		StartCoroutine("GenerateLevel");
    //}

    internal IEnumerator GenerateLevel()//ModDataRoom.GeneratedMission mis)
	{
		Clean();
		WaitForSeconds startup = new WaitForSeconds(.1f);
		WaitForFixedUpdate interval = new WaitForFixedUpdate();

		yield return startup;

		// Place start room
		PlaceStartRoom();


		//place pathway
		for (int i = 1; i <=20+mission.aditionalLength; i++)
		{

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


		//finish the level
		PlaceCorridor(20);

		if (!PlaceEndRoom()) ResetLevelGenerator(); 

		//fill the rest of the level
		FillEmptyDoors();
		ConnectRoomActivator();
		isGenerated = true;
	}
	void ConnectRoomActivator()
    {
		foreach(Room room in placedRooms)
        {
			var x = room.gameObject.GetComponentInChildren<RoomActivator>();
            if (x)
            {
				x.mission = mission;
            }
        }
    }
    internal void Clean()
    {
		// Delete all rooms
		if (startRoom) Destroy(startRoom.gameObject);
		if (endRoom) Destroy(endRoom.gameObject);


		foreach (Room room in placedRooms) Destroy(room.gameObject);
		foreach (var wall in placedWalls) Destroy(wall.gameObject);

		// Clear lists
		placedRooms.Clear();
		placedWalls.Clear();

		availableDoorways.Clear();
		otherDoorways.Clear();

		isGenerated = false;
	}

    private void FillEmptyDoors()
    {
        foreach(Doorway door in otherDoorways)
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
					placedSideRooms.Add(room);

					var specialRoomChance = 25 + mission.increasedChanceSpecialRooms;
					if (Random.Range(1f, 100f) > 100 - specialRoomChance) room.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Special;
					else room.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Encounter;

					break;
				}
			}
		}
		List<int> done = new List<int>();
		int i = 0;
        while (true)
        {
			var rando = Random.Range(0, placedSideRooms.Count);
			if (!done.Contains(rando))
            {
				done.Add(rando);
				i++;
				if (ChangeToNextMain(placedSideRooms[rando])) break;
            }
        }

    }

	private RoomActivator.MainType[] mainTypeList = {RoomActivator.MainType.choiceSpecial, RoomActivator.MainType.itemOrDrop, RoomActivator.MainType.redOrBlue, RoomActivator.MainType.Shop, RoomActivator.MainType.switchInfluence };
	private int mainCounter = 0;
    private bool ChangeToNextMain(Room room)
    {
		room.GetComponentInChildren<RoomActivator>().mainType = mainTypeList[mainCounter];
		room.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Main;
		mainCounter++;

		if (mainCounter >= System.Enum.GetValues(typeof(RoomActivator.MainType)).Length - 1)
		{
			mainCounter = 0;
			return true;
		}
		else return false;
    }

    private bool PlaceCorridor(int length)
    {
		PlaceRoom(stairs, false, RoomType.Corridor);
		return PlaceRoom(corridorPrefabs[Random.Range(0, corridorPrefabs.Count)], true, RoomType.Corridor);
	}

    private bool PlaceEndRoom()
    {
		// Instantiate room
		endRoom = Instantiate(endRoomPrefab) as Room;
		endRoom.transform.parent = this.transform;
		int attempts = 0;
        while (true)
        {
			var randomDoor = endRoom.doorways[Random.Range(0, endRoom.doorways.Length)];
			PositionRoomAtDoorway(ref endRoom, randomDoor , nextDoorway);
			if (!CheckRoomOverlap(endRoom))
			{
				UpdateDoors(endRoom, randomDoor, true);
				return true;
			}
			else attempts++;

			if (attempts > 10) return false;
        }

	}

	private ModDataRoom.GeneratedMission mission;

	internal void StartRun(ModDataRoom.GeneratedMission mis)
    {
		mission = mis;
		StartCoroutine(GenerateLevel()); 
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
					UpdateDoors(currentRoom, door, false);
					placedRooms.Add(currentRoom);
					if (type == RoomType.MainRoom)currentRoom.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Encounter;
					else if (type == RoomType.Corridor )
					{
						if(currentRoom.transform.GetComponentInChildren<RoomActivator>() !=null)
							currentRoom.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Corridor;
					}
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

    private void UpdateDoors(Room currentRoom, Doorway door, bool isFinal)
    {
		bool nexDoorPicked = false;

		foreach (Doorway currentDoor in currentRoom.doorways)
		{
			if (!GameObject.ReferenceEquals(currentDoor.gameObject, door.gameObject))
			{
				if (nexDoorPicked || isFinal) otherDoorways.Add(currentDoor);
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



		Clean();


	

        // Reset coroutine
        StartCoroutine("GenerateLevel");
    }
}



