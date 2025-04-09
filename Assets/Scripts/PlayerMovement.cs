using UnityEngine;
using Listeners;
using Model;
using Utils;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    public event Action<Vector3> OnMovement;

    public bool IsOpponent { get; set; }

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public void MovePlayer(Vector3 newPos)
    {
        controller.Move(newPos * moveSpeed * Time.deltaTime);
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (IsOpponent) return;

        //if (!Input.anyKeyDown) return;

        // Check if player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // keep player grounded
        }

        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Move the player
        //controller.Move(move * moveSpeed * Time.deltaTime);
        MovePlayer(move);

        OnMovement?.Invoke(move);
        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}