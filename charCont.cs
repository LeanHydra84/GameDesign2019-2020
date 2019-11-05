using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charCont : MonoBehaviour
{

    //Values
    public bool debug = false;
    public bool cameraFollow;
    public bool cameraRotate;
    public float walkSpeed = 3;
    public float runMult = 1.6f;
    public float jumpForce = 8f;
    private float runSpeed;
    const float gravity = 20f;
    private bool canMoveOnTransition;

    //Camera Values
    public Vector3 offset; //Math will be needed to fix the offset when the camera turns into rooms
    public float smoothness = 10f;

    //References
    public static Camera mainCam;
    private CharacterController cc;

    //Vector Creation
    Vector3 moveDir = Vector3.zero;
    float mvY;
    float mvX;
    float mvZ;
    public Vector3 walkDirection;
    private Camera[] allCams;
    public bool airStrafing = false;

    private Collider c1;


    void EnableCamera(Camera c)
    {
        for (int i = 0; i < allCams.Length; i++)
        {
            allCams[i].enabled = false;
        }
        c.enabled = true;
    }

    private void Awake()
    {
        allCams = Camera.allCameras;
        EnableCamera(Camera.main);
    }

    void Start()
    {
        mainCam = Camera.main;
        runSpeed = walkSpeed * runMult;
        cc = GetComponent<CharacterController>();
        //mainCam.transform.position = transform.position + offset;
    }

    public void ChangeCamera(roomClass rc) //Allows camera change to be called by any script with the same method
    {
		walkDirection = rc.md;
		EnableCamera(rc.roomCam);
		Debug.Log(rc.roomName);
    }

    void OnTriggerEnter(Collider other)
    {
		Debug.Log(other.gameObject.name);
        if(other == c1) 
        {
            StartCoroutine(lerpCamera(other.GetComponent<roomClass>().roomCam.transform));
            c1 = other;
        }
    }

    void OnGUI() // INFO
    {
        if(debug)
        {
            string editString =
            "AIRSTRAFE = " + airStrafing.ToString().ToUpper() +
            "\nCAM_OFFSET = " + offset +
            "\nCAM_SMOOTH = " + smoothness +
            "\nCONST_GRAVITY = " + gravity +
            "\nMOVE_SPEED = " + walkSpeed +
            "\nRUN_SPEED = " + runSpeed +
            "\nPLAYER_ISRUNNING = " + Input.GetKey(KeyCode.LeftShift).ToString().ToUpper() +
            "\nPLAYER_ISGROUNDED = " + cc.isGrounded.ToString().ToUpper() +
            "\nMOVE_VECTOR = " + moveDir +
            "\nTIME = " + PlayerState.Seconds;
            GUI.skin.textArea.active.background =
            GUI.skin.textArea.normal.background =
            GUI.skin.textArea.onHover.background =
            GUI.skin.textArea.hover.background =
            GUI.skin.textArea.onFocused.background =
            GUI.skin.textArea.focused.background = null;
            GUI.TextArea(new Rect(10, 50, 400, 400), editString);
        }
    }

    IEnumerator lerpCamera(Transform t)
    {
        float startTime = Time.time;
        float journeyDistance = Vector3.Distance(t.position, transform.position);
        float speed = .1f;
		canMoveOnTransition = false;
	
        while(Time.time - startTime < 2f)
        {
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, t.rotation, (Time.time - startTime) * speed);
	    	mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, t.position, (Time.time - startTime * speed));
            yield return 0;
        }
		yield return new WaitForSeconds(0.5f);
		canMoveOnTransition = true;
    }

    void Update()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; //Sprinting

        if (airStrafing || cc.isGrounded) mvX = mvZ = mvY = 0;

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) mvY = jumpForce; //Jumping

        //Movement

        float zSpeed = walkDirection.z * speed;
        float xSpeed = walkDirection.x * speed;

        if ((cc.isGrounded || airStrafing) && canMoveOnTransition)
        {
            if (Input.GetKey(KeyCode.W)) mvZ -= zSpeed; //Forwards
            if (Input.GetKey(KeyCode.S)) mvZ += zSpeed; //Back
            if (Input.GetKey(KeyCode.D)) mvX -= xSpeed; //Right
            if (Input.GetKey(KeyCode.A)) mvX += xSpeed; //Left
        }

        //Handling Gravity
        mvY -= gravity * Time.deltaTime;

        //Making the move
        moveDir = new Vector3(mvX, mvY, mvZ);
        cc.Move(moveDir * Time.deltaTime);

    }

    void LateUpdate() //Camera movement stuff
    {
        //Could save this transform as a variable rather than just saying transform
        //This would let me pass in another object for the camera to target if necessary
        if (cameraFollow)
        {
            Vector3 toPosition = transform.position + offset;
            Vector3 smoothPos = Vector3.Lerp(mainCam.transform.position, toPosition, smoothness * Time.deltaTime);
            mainCam.transform.position = smoothPos;
        }

        if (cameraRotate) mainCam.transform.LookAt(transform);

    }

}
