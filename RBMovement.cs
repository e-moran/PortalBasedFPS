using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RBMovement : NetworkBehaviour
{
	public float speed = 10.0f;
	public float jumpHeight = 8.0f;
	public float sprintMultiplier = 2.0f;
	public float mouseSensitivity = 0.2f;
	public float airMovementModifier = 0.25f;
	public float distToGround = 1.0f;

	private Rigidbody rb;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 bodyRotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;
	private Transform cameraTransform;
	private bool blinked = false;

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		cameraTransform = transform.GetChild(0);
		if (isLocalPlayer)
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(true); // Enable the camera on the correct player
		}
	}

    void Update()
    {
	    if (!isLocalPlayer)
	    {
		    return;
	    }

	    bodyRotation = new Vector3(0, Input.GetAxis("Camera X"), 0) * mouseSensitivity;
	    cameraRotation = new Vector3(Input.GetAxis("Camera Y"), 0, 0) * mouseSensitivity;
	    if (Math.Abs(cameraRotation.y) > 90) // We need to make sure the camera doesn't go past 90 and turn upside down
	    {
		    cameraRotation.y = 90 * Math.Sign(bodyRotation.y);
	    }

	    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
	    moveDirection = transform.TransformDirection(moveDirection); // Deal with rotation
	    moveDirection *= speed;

	    if (IsGrounded())
	    {
		    if (Input.GetButton("Sprint"))
		    {
			    moveDirection *= sprintMultiplier;
		    }

		    if (Input.GetButtonDown("Jump"))
		    {
			    rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
		    }
	    }

	    Debug.Log(IsGrounded());
    }

    void FixedUpdate()
    {
	    rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
	    transform.Rotate(bodyRotation);
	    cameraTransform.Rotate(cameraRotation);
    }

    private bool IsGrounded()
    {
	    return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
