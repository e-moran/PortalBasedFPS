using System;
using Mirror;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 2000.0f;
    public float sprintMultiplier = 2.0f;
    public float mouseSensitivity = 0.2f;
    public float airMovementModifier = 0.25f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3 .zero;
    private Transform cameraTransform;
    private bool blinked = false;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
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

	    if (Input.GetButtonDown("Blink"))
	    {
		    controller.enabled = false; // Temporarily disabled controller to allow for blinking
		    transform.position += new Vector3(blinked ? -100 : 100, 0, 0);
		    controller.enabled = true;
		    blinked = !blinked;
	    }


	    transform.Rotate(new Vector3(0, Input.GetAxis("Camera X"), 0) * mouseSensitivity);
	    cameraTransform.Rotate(new Vector3(Input.GetAxis("Camera Y"), 0, 0) * mouseSensitivity);


	    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
	    moveDirection = transform.TransformDirection(moveDirection); // Deal with rotation
	    moveDirection *= speed;

	    if (controller.isGrounded)
	    {
		    if (Input.GetButton("Sprint"))
		    {
			    moveDirection *= sprintMultiplier;
		    }

		    if (Input.GetButton("Jump"))
		    {
			    moveDirection.y = jumpSpeed;
		    }
	    }

	    moveDirection.y -= gravity * Time.deltaTime; // Gravity
	    Debug.Log(gravity * Time.deltaTime);

	    controller.Move(moveDirection * Time.deltaTime);
    }
}
