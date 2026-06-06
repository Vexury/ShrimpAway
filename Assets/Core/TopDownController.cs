using UnityEngine;

public class TopDownController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;

    private Vector2 moveInput;
    private bool isSprinting;
    private float verticalVelocity;

    private void Awake()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        inputReader.EnableGameplayInput();
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.SprintEvent += OnSprint;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.SprintEvent -= OnSprint;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * currentSpeed;

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        characterController.Move(movement * Time.deltaTime);
    }

    private void OnMove(Vector2 input) => moveInput = input;
    private void OnSprint(bool isPressed) => isSprinting = isPressed;
}