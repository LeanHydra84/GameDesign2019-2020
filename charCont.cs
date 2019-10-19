using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charCont : MonoBehaviour {
	
	//Values
	public float walkSpeed = .5f;
	public float runMult = 2;
	public float jumpForce = 8f;
	private float runSpeed;
	const float gravity = 20f;
	
	//Camera Values
	public Vector3 offset; //Math will be needed to fix the offset when the camera turns into rooms
	public float smoothness = 10f;
	
	//References
	private Camera mainCam;
	private CharacterController cc;
	
	//Vector Creation
	Vector3 moveDir = Vector3.zero;
	float mvY;
	float mvX;
	float mvZ;

    public bool airStrafing = true;

	void Start() 
	{
		mainCam = Camera.main;
		runSpeed = walkSpeed * runMult;
		cc = GetComponent<CharacterController>();
        mainCam.transform.position = transform.position + offset;
	}

    void Update() 
	{
		float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; //Sprinting
		
		if(airStrafing || cc.isGrounded) mvX = mvZ = mvY = 0;
		if(Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) mvY = jumpForce; //Jumping
		
		//Movement
        if(cc.isGrounded || airStrafing)
        {
            if(Input.GetKey(KeyCode.W)) mvZ -= speed; //Forwards
		    if(Input.GetKey(KeyCode.S)) mvZ += speed; //Back
		    if(Input.GetKey(KeyCode.D)) mvX -= speed; //Right
		    if(Input.GetKey(KeyCode.A)) mvX += speed; //Left
        }
		
		
		//Handling Gravity
		mvY -= gravity * Time.deltaTime;
		moveDir = new Vector3(mvX, mvY, mvZ);
		
		//Making the move
		cc.Move(moveDir * Time.deltaTime);
		
	}

    private void OnGUI() // INFO
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

    void LateUpdate() //Camera movement stuff
	{
		//Could save this transform as a variable rather than just saying transform
		//This would let me pass in another object for the camera to target if necessary
		Vector3 toPosition = transform.position + offset; 
		Vector3 smoothPos = Vector3.Lerp(mainCam.transform.position, toPosition, smoothness * Time.deltaTime);
		mainCam.transform.position = smoothPos;
		
		mainCam.transform.LookAt(transform);
		
	}
	
}
