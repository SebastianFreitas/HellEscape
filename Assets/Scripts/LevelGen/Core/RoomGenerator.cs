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

	[SerializeField] internal GameObject choseSpecial;
	[SerializeField] internal GameObject itemRoom;
	[SerializeField] internal GameObject changeInfluence;
	[SerializeField] internal GameObject shop;
	[SerializeField] internal GameObject influenceItem;

	[SerializeField] internal GameObject healthPack;
	[SerializeField] internal GameObject CraftingBench;
	[SerializeField] internal GameObject weaponDrop;

	[SerializeField]internal Monster[] monstersRed;
	[SerializeField]internal Monster[] monstersBlue;
					
	[SerializeField]internal Monster[] BossRed;
	[SerializeField] internal Monster[] BossBlue;


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
	internal VoidBoon.BoonType influence = VoidBoon.BoonType.Red;

	internal ModDataRoom.GeneratedMission mission;

	internal int encounterCounter = 0;
	internal List<List<int>> encounters = new List<List<int>>();

	[SerializeField] bool startAtBoss = false;
	internal IEnumerator GenerateLevel()
	{
		WaitForSeconds startup = new WaitForSeconds(.2f);
		WaitForFixedUpdate interval = new WaitForFixedUpdate();

		weaponLevel = playerInv.weaponLevel;
		player.SetActive(false);
		yield return startup;
		Clean();

		yield return interval;

		// Place start room
		PlaceStartRoom();
		yield return interval;
		

		
		int influencePos = Random.Range(1,5);
		//place pathway
		for (int i = 1; i <=mission.totalLenght + mission.aditionalLength; i++) //
		{

			int a = 0;
            while (true)
            {
				a++;
				yield return interval;

				var rando = Random.Range(0, mainRooms.Count);

				if (i == influencePos)
				{
					if (PlaceRoom(mainRooms[rando], true, RoomActivator.RoomType.Main))
					{
						influencePos += 20;
						break;
					}


                }
                else
                {
					var x = Random.Range(1, 4);
					bool worked;
					if (x >= 2) worked = PlaceCorridor(Random.Range(5, 10));
					else
					{
						worked = PlaceRoom(mainRooms[rando], true, RoomActivator.RoomType.Encounter);
					}

					if (worked) break;
                }



				if (a > 10)
				{
					Debug.Log(i+" "+ influencePos);
					ResetLevelGenerator();
				}

			}
			yield return interval;

		}


		//finish the level
		PlaceCorridor(20);

		if (!PlaceEndRoom()) ResetLevelGenerator();
		yield return interval;

		//fill the rest of the level
		FillEmptyDoors();
		ConnectRoomActivator();

		yield return interval;
		//for (int i = 1; i < placedRooms.Count; i++)
		//	placedRooms[i].gameObject.SetActive(false);

		//startRoom.gameObject.SetActive(true);

		if (startAtBoss) currentStartPos = endRoom.GetComponentsInChildren<RoomActivator>()[0].spawnPos.position;

		player.SetActive(true);
		player.GetComponent<CharacterController>().enabled = false;
		player.transform.position = currentStartPos;
		player.GetComponent<CharacterController>().enabled = true;

		ApplyMissionToPlayer(true);
		ApplyMission();

		CreateMainRooms();

		yield return interval;
		isGenerated = true;
		started = false;
	}

    private void ApplyMission()
    {
        for(int i = 0; i < mission.additionalBoons; i++)
        {
			var room = placedRooms[Random.Range(6, placedRooms.Count)];
			var roomAct = room.GetComponentInChildren<RoomActivator>();

			if(roomAct != null)
            {
				if (roomAct.roomType != RoomActivator.RoomType.Corridor &&
					roomAct.roomType != RoomActivator.RoomType.Boss		&&
					roomAct.mainType != RoomActivator.MainType.itemOrDrop) roomAct.roomType = RoomActivator.RoomType.Boon;
				else i--;	
            }
			else i--;


		}

        if (mission.additionalRewards)
        {
			mainTypeList.Add(RoomActivator.MainType.choiceSpecial);
			mainTypeList.Add(RoomActivator.MainType.itemOrDrop);
			mainTypeList.Add(RoomActivator.MainType.Shop);
			mainTypeList.Add(RoomActivator.MainType.redOrBlue);

		}

        if (mission.error)
        {
			foreach(var current in placedRooms)
            {
				
				var x = current.GetComponentInChildren<RoomActivator>();
				if (x.roomType != RoomActivator.RoomType.Corridor) x.roomType = (RoomActivator.RoomType)Random.Range(0, System.Enum.GetValues(typeof(RoomActivator.RoomType)).Length);
			}
        }

    }

    public void Awake()
	{

		player = GameObject.FindGameObjectsWithTag("Dude")[0];
		playerHP = player.GetComponent<PlayerHpManager>();
		playerInv = player.GetComponent<PlayerInventory>();
		playerMov = player.GetComponent<PlayerBasicMovement>();


		encounters.Add(new List<int> { 0, 0, 0, 0 });
		encounters.Add(new List<int> { 0, 0, 0, 1 });
		encounters.Add(new List<int> { 0, 0, 1, 1 });
		encounters.Add(new List<int> { 0, 1, 1, 1 });
		encounters.Add(new List<int> { 1, 1, 1, 1 });
		//encounters.Add(new List<int> { 0, 0, 0, 2 });
		//encounters.Add(new List<int> { 1, 1, 2, 2 });
		//encounters.Add(new List<int> { 1, 2, 2, 2 });

	}

	internal PlayerBasicMovement playerMov;
	internal PlayerHpManager playerHP;
	internal PlayerInventory playerInv;
	internal GameObject player;

	internal void ApplyMissionToPlayer(bool isAdd)
    {
		if (mission != null)
        {
			if (isAdd)
			{
				player.GetComponentInChildren<GrenadeHolder>().possible = mission.noGrenade;
				playerMov.increasedSpeed -= mission.playerReducedMovementSpeed;
			}
			else
			{
				player.GetComponentInChildren<GrenadeHolder>().possible = true;
				playerMov.increasedSpeed += mission.playerReducedMovementSpeed;

			}
        }
    }

    internal void ManageRoomsEficiency(Room room)
    {
		int index = placedRooms.FindIndex(a => (GameObject.ReferenceEquals(a.gameObject, room.gameObject)));

		for(int i = 0; i < placedRooms.Count; i++)
			placedRooms[i].gameObject.SetActive(!(i + 4 <= index || i - 4 >= index));

        
    }

    void ConnectRoomActivator()
    {
		foreach(Room room in placedRooms)
        {
			var x = room.gameObject.GetComponentInChildren<RoomActivator>();
            if (x)
            {
				x.gameObject.SetActive(true);
				x.mission = mission;

				x.influcence = influence;
			}
        }

        foreach (Room room in placedSideRooms)
        {
            var x = room.gameObject.GetComponentInChildren<RoomActivator>();
            if (x)
            {                
				x.gameObject.SetActive(true);
				x.influcence = influence;
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
		foreach (Room room in placedSideRooms) Destroy(room.gameObject);
		foreach (var wall in placedWalls) Destroy(wall.gameObject);

		// Clear lists
		placedRooms.Clear();
		placedWalls.Clear();
		placedSideRooms.Clear();

		availableDoorways.Clear();
		otherDoorways.Clear();
		counter = 0;
		isGenerated = false;

		foreach (Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

    private void FillEmptyDoors()
    {
        foreach (Doorway door in otherDoorways)
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
                    //placedRooms.Add(room);
                    placedSideRooms.Add(room);

                    var x = room.transform.GetComponentInChildren<RoomActivator>();
                    if (x != null)
                    {
                        x.isSideRoom = true;
                        var specialRoomChance = 25 + mission.increasedChanceSpecialRooms;
                        if (Random.Range(1f, 100f) > 100 - specialRoomChance) room.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Special;
                        else room.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Encounter;
                    }


                    break;
                }
            }
        }



    }

    private void CreateMainRooms()
    {
        List<int> done = new List<int>();
        int i = 0;
        while (true)
        {
            //var rando = Random.Range(0, placedSideRooms.Count);
            //if (!done.Contains(rando))
            //{
            //    done.Add(rando);
            //    i++;
            //    if (ChangeToNextMain(placedSideRooms[rando])) break;
            //}

			if (i >= placedSideRooms.Count) break;

            for (int a = 0; a < mainTypeList.Count; a++)
            {
				if (ChangeToNextMain(placedSideRooms[i])) i += Random.Range(5, 10);
				else i++; 
			}
        }
    }

    private List<RoomActivator.MainType> mainTypeList =  new List<RoomActivator.MainType>() {RoomActivator.MainType.choiceSpecial, RoomActivator.MainType.itemOrDrop, RoomActivator.MainType.Shop };
	private int mainCounter = 0;
    private bool ChangeToNextMain(Room room)
    {
		room.GetComponentInChildren<RoomActivator>().mainType = mainTypeList[mainCounter];
		room.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Main;
		mainCounter++;

		if (mainCounter >=mainTypeList.Count) //  System.Enum.GetValues(typeof(RoomActivator.MainType)).Length -1
		{
			mainCounter = 0;
			return true;
		}
		else return false;
    }

    private bool PlaceCorridor(int length)
    {
		PlaceRoom(stairs, false, RoomActivator.RoomType.Corridor);
		 PlaceRoom(corridorPrefabs[Random.Range(0, corridorPrefabs.Count)], true, RoomActivator.RoomType.Corridor);
		return PlaceRoom(stairs, false, RoomActivator.RoomType.Corridor);
	}

    private bool PlaceEndRoom()
    {
		// Instantiate room
		endRoom = Instantiate(endRoomPrefab) as Room;
		endRoom.transform.parent = this.transform;
		placedRooms.Add(endRoom);
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


    private int counter = 0;
	private bool started = false;
	internal int weaponLevel;
    internal int mirrorLength = 0;
    internal float mirrorBoonChance = 0;

    internal void StartRun(ModDataRoom.GeneratedMission mis)
    {
		mission = mis;
		weaponLevel += mis.additionalWeaponLevel;
		if (!started)
		{
			started = true;
			StartCoroutine("GenerateLevel");
		}

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
		startRoom.transform.GetComponentInChildren<RoomActivator>().roomType = RoomActivator.RoomType.Corridor;

		placedRooms.Add(startRoom);

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

	bool PlaceRoom(Room room, bool shuffle, RoomActivator.RoomType type)
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

					
					var x = currentRoom.transform.GetComponentInChildren<RoomActivator>();
					if (x)
                    {
						if (type == RoomActivator.RoomType.Main && counter == 0)
						{
							counter++;
							x.mainType = RoomActivator.MainType.switchInfluence;
						}
						x.roomType = type;
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

		List<Room> rooms = new List<Room>();
		rooms.AddRange(placedRooms);
		rooms.AddRange(placedSideRooms);
		foreach (Room currentRoom in rooms)
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

		isGenerated = false;

		Debug.LogError("Reset level generator");

		StopCoroutine("GenerateLevel");

		// Reset coroutine
		StartCoroutine("GenerateLevel");
        

    }

	internal void ChangeInfluence(VoidBoon.BoonType influence)
    {
		foreach(var current in placedRooms)
        {
			var x = current.GetComponentInChildren<RoomActivator>();
			if (x != null)
            {
				x.influcence = influence;
				if (x.beenTrigered)
                {

					x.TurnLightsRed();
                }



			}
        }

		foreach (var current in placedSideRooms)
		{
			var x = current.GetComponentInChildren<RoomActivator>();
			if (x != null)
			{
					x.influcence = influence;
				if (x.beenTrigered)
				{

					x.TurnLightsRed();
				}

			}
		}

		this.influence = influence;
	}
}



