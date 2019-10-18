using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charCont : MonoBehaviour {
	
	public float walkSpeed = .5f;
	public float runSpeed = 3f;
	
	CharacterController cc;
	const float gravity = 20f;
	const float jumpForce = 8f;
	Vector3 moveDir = Vector3.zero;
	
	float mvY;
	float mvX;
	float mvZ;
	
	void Start() 
	{
		cc = GetComponent<CharacterController>();
	}
	
	void Update() 
	{
		float speed = Input.GetKey(KeyCode.LShift) ? runSpeed : walkSpeed; //Sprinting

		mvX = mvZ = 0;
		if(Input.GetButtonDown("jump") && cc.isGrounded) mvY = jumpforce; //Jumping
		
		//Movement
		if(Input.GetKey(KeyCode.W)) mvX += speed; //Forwards
		if(Input.GetKey(KeyCode.S)) mvX -= speed; //Back
		if(Input.GetKey(KeyCode.D)) mvZ += speed; //Right
		if(Input.GetKey(KeyCode.A)) mvZ -= speed; //Left
		
		//Handling Gravity
		mvY -= gravity * Time.deltaTime;
		moveDir = new Vector3(mvX, mvY, mvZ);
		
		//Making the move
		cc.Move(moveDir * Time.deltaTIme);
		
	}
	
}
