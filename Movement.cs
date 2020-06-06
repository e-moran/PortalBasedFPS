using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 24.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float sprintMultiplier = 2.0f;
    public float mouseSensitivity = 0.2f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Transform cameraTransform;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = transform.GetChild(0);
    }
    
    void Update()
    {
        transform.Rotate(new Vector3(0,  Input.GetAxis("Camera X"), 0) * mouseSensitivity);
        cameraTransform.Rotate(new Vector3(Input.GetAxis("Camera Y"),  0, 0) * mouseSensitivity);
        
        
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection); // Deal with rotation
            moveDirection *= speed;

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
        
        controller.Move(moveDirection * Time.deltaTime);
    }
}
