using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTrap : MonoBehaviour
{
    private LineRenderer lr;

    [SerializeField] private bool isRotating;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        lr = GetComponent<LineRenderer>();  

    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);

        Vector3 to = new Vector3(-1, 0, 0);
        if (isRotating)
        {
            Debug.Log(transform.rotation.y);
            if (transform.rotation.y < .99f)
            {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), rotationSpeed * Time.time);
            }
             else transform.rotation = Quaternion.Euler(0, 0, 0);
            

        }

        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))//, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }

            if(hit.transform.CompareTag("Dude"))
            {
                hit.transform.GetComponent<PlayerHpManager>().TakeDamage(10f);
            }

        }
    }
}
