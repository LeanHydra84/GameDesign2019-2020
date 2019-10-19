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
	
	void Start() 
	{
		mainCam = Camera.main;
		runSpeed = walkSpeed * runMult;
		cc = GetComponent<CharacterController>();
	}
	
	void Update() 
	{
		float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; //Sprinting
		
		mvX = mvZ = 0;
		if(Input.GetButtonDown("jump") && cc.isGrounded) mvY = jumpForce; //Jumping
		
		//Movement
		if(Input.GetKey(KeyCode.W)) mvX += speed; //Forwards
		if(Input.GetKey(KeyCode.S)) mvX -= speed; //Back
		if(Input.GetKey(KeyCode.D)) mvZ += speed; //Right
		if(Input.GetKey(KeyCode.A)) mvZ -= speed; //Left
		
		//Handling Gravity
		mvY -= gravity * Time.deltaTime;
		moveDir = new Vector3(mvX, mvY, mvZ);
		
		//Making the move
		cc.Move(moveDir * Time.deltaTime);
		
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
