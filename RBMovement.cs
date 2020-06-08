using Mirror;
using UnityEngine;

public class RBMovement : NetworkBehaviour
{
	public float speed = 10.0f;
	public float jumpHeight = 8.0f;
	public float sprintMultiplier = 2.0f;
	public float mouseSensitivity = 0.2f;
	public float distToGround = 1.0f;

	private Rigidbody rb;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 bodyRotation = Vector3.zero;
	private Transform cameraTransform;
	private bool blinked = false;
	private float cameraRot = 0f;

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

	    if (Input.GetButtonDown("Blink"))
	    {
		    transform.position += new Vector3(blinked ? -100 : 100, 0, 0); // Switch between adding and subbing 100
		    blinked = !blinked;
	    }

	    bodyRotation = new Vector3(0, Input.GetAxis("Camera X"), 0) * (mouseSensitivity * Time.deltaTime);
	    cameraRot += Input.GetAxis("Camera Y") * mouseSensitivity * Time.deltaTime;
	    cameraRot = Mathf.Clamp(cameraRot, -90, 90); // To prevent the camera from flipping

	    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
	    moveDirection = transform.TransformDirection(moveDirection); // Make sure forward takes in to account the direction the player is facing
	    moveDirection *= speed;

	    // If the player is sprinting we apply a multiplier to the player's speed
	    if (Input.GetButton("Sprint"))
	    {
		    moveDirection *= sprintMultiplier;
	    }

	    if (IsGrounded())
	    {
		    if (Input.GetButtonDown("Jump"))
		    {
			    rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -1f * Physics.gravity.y), ForceMode.VelocityChange);
		    }
	    }
    }

    void FixedUpdate()
    {
	    rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
	    transform.Rotate(bodyRotation);
	    cameraTransform.localEulerAngles = new Vector3(cameraRot, 0, 0);
    }

    private bool IsGrounded()
    {
	    return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }
}
