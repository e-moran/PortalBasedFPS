using Mirror;
using UnityEngine;

public class RBMovement : NetworkBehaviour
{
	public float speed = 10.0f;
	public float jumpHeight = 8.0f;
	public float sprintMultiplier = 2.0f;
	public float mouseSensitivity = 0.2f;
	public float distToGround = 1.0f;

	private Rigidbody _rb;
	private GameStateManager _stateManager;
	private Vector3 _moveDirection = Vector3.zero;
	private Vector3 _bodyRotation = Vector3.zero;
	private Transform _cameraTransform;
	private bool _blinked = false;
	private float _cameraRot = 0f;

	void Start()
	{
		_stateManager = GetComponent<GameStateManager>();
		_rb = gameObject.GetComponent<Rigidbody>();
		_cameraTransform = transform.GetChild(0);
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
		    transform.position += new Vector3(_blinked ? -100 : 100, 0, 0); // Switch between adding and subbing 100
		    _blinked = !_blinked;
	    }

	    _bodyRotation = new Vector3(0, Input.GetAxis("Camera X"), 0) * mouseSensitivity;
	    _cameraRot += Input.GetAxis("Camera Y") * mouseSensitivity;
	    _cameraRot = Mathf.Clamp(_cameraRot, -90, 90); // To prevent the camera from flipping

	    _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
	    _moveDirection = transform.TransformDirection(_moveDirection); // Make sure forward takes in to account the direction the player is facing
	    _moveDirection *= speed;

	    if (IsGrounded())
	    {
		    // If the player is sprinting we apply a multiplier to the player's speed
		    if (Input.GetButton("Sprint"))
		    {
			    _moveDirection *= sprintMultiplier;
		    }

		    if (Input.GetButtonDown("Jump"))
		    {
			    _rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
		    }
	    }
    }

    void FixedUpdate()
    {
	    // Ensure that the player is in-game before attempting to move them
	    if (_stateManager.State == GameStateManager.GameState.IN_GAME)
	    {
		    _rb.MovePosition(_rb.position + _moveDirection * Time.fixedDeltaTime);
		    transform.Rotate(_bodyRotation);
		    _cameraTransform.localEulerAngles = new Vector3(_cameraRot, 0, 0);
	    }
    }

    private bool IsGrounded()
    {
	    return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }
}
