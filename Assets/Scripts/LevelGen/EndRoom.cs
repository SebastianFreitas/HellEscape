
using UnityEngine;

public class EndRoom : Room
{
    private void Start()
    {
        var activator = GetComponentInChildren<RoomActivator>();
        activator.roomType = RoomActivator.RoomType.Boss;
        activator.spawnPos = activator.transform.GetChild(0);
    }
}
