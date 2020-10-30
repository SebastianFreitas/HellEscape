using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{
  private Camera playerView;
  private Vector3 dir;
  private float x = Screen.width / 2;
  private float y = Screen.height / 2;
  private Ray ray;

    void Start()
    {
      playerView = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      dir = playerView.ScreenPointToRay(new Vector3(x, y, 0)).direction;
      StartCoroutine(waiter());
    }

    void Update()
    {
        transform.position += dir*Time.deltaTime*50;
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(5f);
      Destroy(gameObject);
    }
}
