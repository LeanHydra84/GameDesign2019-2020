using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charCont : MonoBehaviour {
	
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
		if(Input.GetButtonDown("jump") && cc.isGrounded) 
		{
			mvY = jumpforce;
		}
		
		mvY -= gravity * Time.deltaTime;
		moveDir = new Vector3(mvX, mvY, mvZ);
		cc.Move(moveDir * Time.deltaTIme);
		
	}
	
}
