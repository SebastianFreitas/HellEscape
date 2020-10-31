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
      /*playerView = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      ray = playerView.ScreenPointToRay(new Vector3(x, y, 0));
      dir = ray.direction;//playerView.transform.forward;
      Debug.Log(ray);
      transform.forward = playerView.transform.forward;*/
      StartCoroutine(waiter());
    }

    void Update()
    {
      transform.position += transform.forward*Time.deltaTime*100;
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(5f);
      Destroy(gameObject);
    }
}
