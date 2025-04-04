using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController playerController;
    public Transform cam;
    public Animator animator;

    public float speed = 6f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    private Vector3 velocity;
    public bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset downward force when grounded
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Handle movement and rotation
        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isRunning", true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Handle Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        // Set falling animation if not grounded
        if (!isGrounded && velocity.y < 0)
        {
            animator.Play("Falling"); // Assumes your falling animation is named "Falling"
        }
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

}
