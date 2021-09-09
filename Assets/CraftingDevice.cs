using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDevice : MonoBehaviour
{
    private GameObject player;
    public TMPro.TextMeshPro guntext;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponentInParent<Room>().player;
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Bullet"))
        {

            guntext.text = collision.transform.GetComponent<PlayerProjectile>().Gun.text;

        }


    }
}
