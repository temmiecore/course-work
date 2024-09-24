using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Probably shouldn't cramp all the code in one script but oh well

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public UIController uiController;

    private CharacterController characterController;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravityStrength = 10f;
    public bool canMove = true;
    public bool canJump = false;
    private bool isMoving = false;
    private bool isRunning = false;

    [Header("Camera Rotation Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Head Bop Settings")]
    public bool headBop = true;
    public float headBopStrength = 0.01f;
    public float headBopWalkSpeed = 10f;
    public float headBopRunSpeed = 30f;
    private float defaultCameraY;
    private float defaultCameraX;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        defaultCameraY = playerCamera.transform.position.y;
        defaultCameraX = playerCamera.transform.position.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();
        HandleInteraction();
        HandleHeadBop();
    }

    void FixedUpdate()
    {
        HandleMovement();
        defaultCameraY = transform.position.y + 0.6f;
        defaultCameraX = transform.position.x;
    }

    void HandleMovement()
    {
        #region Movement handler
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #endregion

        #region Jumping handler
        if (canJump && Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravityStrength * Time.fixedDeltaTime;
        }
        #endregion

        if (curSpeedX != 0 || curSpeedY != 0)
            isMoving = true;
        else
            isMoving = false;

        characterController.Move(moveDirection * Time.fixedDeltaTime);
    }

    void HandleCameraRotation()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    void HandleInteraction() 
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        float sphereRadius = 0.5f;
        float maxDistance = 3f;
        // Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue, 10f);

        if (Physics.SphereCast(ray, sphereRadius, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Item"))
                uiController.setUIInteractionPopup("Pick up? E");
            if (hit.collider.CompareTag("Door"))
                uiController.setUIInteractionPopup("Go in? E");
            // Shop? "Buy"
            else
                uiController.setUIInteractionPopup("");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.SphereCast(ray, sphereRadius, out hit, maxDistance))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    Item item = hit.collider.gameObject.GetComponent<Item>();
                    if (item != null)
                        item.PickUp();
                }

                if (hit.collider.CompareTag("Door")) 
                {
                    SceneChanger sceneChanger = hit.collider.gameObject.GetComponent<SceneChanger>();
                    sceneChanger.ChangeScene();
                }
            }

        }
    }

    void HandleHeadBop() 
    {
        if (headBop)
            if (isRunning)
                playerCamera.transform.position = new Vector3
                    (
                    playerCamera.transform.position.x, //playerCamera.transform.position.x + Mathf.Cos(Time.time * headBopRunSpeed / 2) * headBopStrength * 0.01f,
                    playerCamera.transform.position.y + Mathf.Sin(Time.time * headBopRunSpeed) * headBopStrength * 0.01f,
                    playerCamera.transform.position.z
                    );
            else if (isMoving)
                playerCamera.transform.position = new Vector3
                    (
                    playerCamera.transform.position.x, //playerCamera.transform.position.x + Mathf.Cos(Time.time * headBopWalkSpeed / 2) * headBopStrength * 0.01f,
                    playerCamera.transform.position.y + Mathf.Sin(Time.time * headBopWalkSpeed) * headBopStrength * 0.01f,
                    playerCamera.transform.position.z
                    );
            else if (characterController.isGrounded && playerCamera.transform.position.y != defaultCameraY)
                playerCamera.transform.position = new Vector3
                    (
                    playerCamera.transform.position.x, //Mathf.Lerp(playerCamera.transform.position.x, defaultCameraX, Time.deltaTime * 5f),
                    Mathf.Lerp(playerCamera.transform.position.y, defaultCameraY, Time.deltaTime * 5f),
                    playerCamera.transform.position.z
                    );
    }
}