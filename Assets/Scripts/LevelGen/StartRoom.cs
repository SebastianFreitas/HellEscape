
using UnityEngine;

public class StartRoom : Room
{
    public Transform playerStart;

    internal void VoidPlayer()
    {
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = playerStart.position;
        player.GetComponent<PlayerHpManager>().TakeDamage(10);
        player.GetComponent<CharacterController>().enabled = true;

    }
}
