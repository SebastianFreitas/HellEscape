using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct convergion
{
  public float bludgeoning;
  public float piercing;
  public float slashing;

}
public class PlayerProjectile : MonoBehaviour
{
    public GameObject damagePopup;
  public convergion totalConvergion;

  public float speed = 500f;
  private  float damage = 0;
  public  Vector3 playerSpeed;
  public AudioClip ricochet;
  public AudioSource source;

  public GameObject newBullet;

  private Monster enemyScript;

  public bool visual = false;

  public bool initialFade = false;

  public int bounces = 2;
  Rigidbody rb;

    

    void Start()
      {
        SetVisibility(false);
        rb = GetComponent<Rigidbody>();

        if (initialFade) StartCoroutine(fadeWaiter());//this line will fuck up (usual bug andar pa tras ou pa frente + double bounce com side walk)
        rb.AddForce(transform.forward * speed);
        StartCoroutine(waiter(10f));
      }


  private void SetVisibility(bool onOff)
  {
    //this.GetComponent<Renderer>().enabled = onOff;
    this.GetComponentInChildren<TrailRenderer>().enabled = onOff;
  }

  IEnumerator waiter(float a){
    yield return new WaitForSeconds(a);
    Destroy(gameObject);
  }

  IEnumerator fadeWaiter()
  {
    SetVisibility(false);
    yield return new WaitForSeconds(.005f);
    SetVisibility(true);
  }

  void OnEnable()
  {
    StartCoroutine(waiter(2f));
  }

  public void SetStats(float damage, int bounces)
  {
    this.damage = damage;
    this.bounces = bounces;
  }

  void OnCollisionEnter(Collision collision)
  {
      ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.transform.GetComponent<Monster>().TakeDamage(damage);

            Destroy(this.gameObject);
        }
        else
        if (collision.gameObject.CompareTag("Prop"))
        {
            collision.transform.GetComponent<Rigidbody>().AddForce(contact.normal * 100);

        }


        if (bounces > 0 )
        {
            SetVisibility(true);
            AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position, 0.2f);
            rb.AddForce(contact.normal * 10);
            bounces--;
        }
        else Destroy(this.gameObject);
  }

    private void DamagePopup(float damage)
    {
        //var rep = player;
        //rep.LookAt(transform.position);

        GameObject currentText = Instantiate(damagePopup);
        TextMesh textMesh = currentText.GetComponentInChildren<TextMesh>();

        currentText.transform.position = rb.position;

        textMesh.text = damage.ToString();
        Destroy(currentText, .3f);
    }







}
