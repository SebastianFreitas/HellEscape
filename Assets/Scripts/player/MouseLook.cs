using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://www.youtube.com/watch?v=_QajrabyTJc&list=PLPV2KyIb3jR7dFbE2UQYu7QWMdUgDnlnk&index=7
//this video explains this
//https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/page-2 for second version
public class MouseLook : MonoBehaviour
{
  /*  public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
      // Hide the cursor
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
      /* Camera rotation stuff, mouse controls this shit */
      /*float mouseX =  Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
      float mouseY =  Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

      xRotation -= mouseY;
      xRotation = Mathf.Clamp(xRotation, -90f, 90f);

      transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
      playerBody.Rotate(Vector3.up * mouseX);
    }*/
      [Header("CameraTransform")]
     public    Transform   targetTrans;
     //public    Transform   playerBody;
      [Header("On/Off & Settings")]
     public    bool        inputActive        = true;
     public    bool        controlCursor    = false;
     public    bool        pitchClamp        = true;
      [Header("Smoothing")]
     public    bool        byPassSmoothing    = false;
     public    float        mLambda            = 20F;    //higher = less latency but also less smoothing
    [Header("Sensitivity")]
    public float hSens;//            = PlayerPrefs.GetFloat("Sensitivity");
    public float vSens;//            = PlayerPrefs.GetFloat("Sensitivity");
    public    BufferV2    mouseBuffer        = new BufferV2();

    private void Awake()
    {
        hSens = PlayerPrefs.GetFloat("Sensitivity");
        vSens = PlayerPrefs.GetFloat("Sensitivity");
    }

    private void OnEnable()
    {
        hSens = PlayerPrefs.GetFloat("Sensitivity");
        vSens = PlayerPrefs.GetFloat("Sensitivity");
    }

    void Update ()
     {
         if(controlCursor){    //Cursor Control
             if(  inputActive && Cursor.lockState != CursorLockMode.Locked)  { Cursor.lockState = CursorLockMode.Locked;  }
             if( !inputActive && Cursor.lockState != CursorLockMode.None)    { Cursor.lockState = CursorLockMode.None;    }
         }
         if(!inputActive){ return; }    //active?

         //Update input
         UpdateMouseBuffer();

     }

     //consider late Update for applying the rotation if your game needs it (e.g. if camera parents are rotated in Update for some reason)
     void LateUpdate() 
     {
         targetTrans.rotation        = Quaternion.Euler( mouseBuffer.curAbs );
     }

     private    void UpdateMouseBuffer()
     {
         mouseBuffer.target    += new Vector2( vSens * -Input.GetAxisRaw("Mouse Y"), hSens * Input.GetAxisRaw("Mouse X") );//Mouse Input is inherently framerate independend!
         mouseBuffer.target.x = pitchClamp?Mathf.Clamp(mouseBuffer.target.x,-90F,+90F) :mouseBuffer.target.x;
         mouseBuffer.Update( mLambda, Time.deltaTime, byPassSmoothing );
     }
  }



  #region Helpers
  [System.Serializable]
  public class BufferV2{

     public BufferV2(){
         this.target = Vector2.zero;
         this.buffer = Vector2.zero;
     }
     public BufferV2( Vector2 targetInit, Vector2 bufferInit ) {
         this.target = targetInit;
         this.buffer = bufferInit;
     }

     /*private*/public    Vector2    target;
     /*private*/public    Vector2    buffer;

     public    Vector2    curDelta;    //Delta: apply difference from lastBuffer state to current BufferState        //get difference between last and new buffer
     public    Vector2    curAbs;        //absolute



     /// <summary>Update Buffer By supplying new target</summary>
     public    void UpdateByNewTarget( Vector2 newTarget, float dampLambda, float deltaTime ){
         this.target        = newTarget;
         Update(dampLambda, deltaTime);
     }
     /// <summary>Update Buffer By supplying the rawDelta to the last target</summary>
     public    void UpdateByDelta( Vector2 rawDelta, float dampLambda, float deltaTime ){
         this.target        = this.target +rawDelta;    //update Target
         Update(dampLambda, deltaTime);
     }

     /// <summary>Update Buffer</summary>
     public    void Update( float dampLambda, float deltaTime, bool byPass = false ){
         Vector2 last    = buffer;            //last state of Buffer
         this.buffer        = byPass? target : DampToTargetLambda( buffer, this.target, dampLambda, deltaTime);    //damp current to target
         this.curDelta    = buffer -last;
         this.curAbs        = buffer;
     }
     public static Vector2        DampToTargetLambda( Vector2    current,        Vector2        target,        float lambda, float dt){
         return Vector2.        Lerp(current, target, 1F -Mathf.Exp( -lambda *dt) );
     }
  }
  #endregion Helpers
