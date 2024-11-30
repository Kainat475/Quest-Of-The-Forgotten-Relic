using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast_jump : MonoBehaviour
{
    public float speed = 5f; // Default horizontal movement speed
    public float sprintSpeed = 20f; // Speed when sprinting
    public float jumpForce = 10f; // Jump force
    public Rigidbody2D rb; // Character's Rigidbody2D
    public Animator animator; // Animator for character animations

    private bool isJumping; // Flag to track jumping state
    private bool isGround; // Flag to track if character is on the ground
    private float move; // Horizontal movement input
    private Vector3 playerscale; // Scale of the player sprite
    private float defaultSpeed; // Store the default speed for resetting
    public int playerHP = 6; // Player's health points

    public LayerMask enemyLayer; // Layer for detecting enemies
    public float attackRange = 1f; // Range of the attack

    void Start()
    {
        playerscale = transform.localScale;
        defaultSpeed = speed; // Store the default speed
    }

    void Update()
    {
        // Get horizontal input
        move = Input.GetAxis("Horizontal");
        Debug.Log("Movement: " + move);

        // Sprint when Shift is held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = defaultSpeed;
        }

        // Flip character sprite based on movement direction
        if (move > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(playerscale.x), playerscale.y, playerscale.z);
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(playerscale.x), playerscale.y, playerscale.z);
        }

        // Update animator parameters
        animator.SetFloat("speed", Mathf.Abs(move));
        isGround = isGrounded();
        animator.SetBool("isGround", isGround);

        // Jump when Space is pressed and character is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Debug.Log("Player jumped!");
            animator.SetTrigger("jumped");
            isJumping = true;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        // Stop jump when Space is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        // Attack enemy when H is pressed
        if (Input.GetKeyDown(KeyCode.H))
        {
            AttackEnemy();
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    bool isGrounded()
    {
        // Cast a ray downwards to check for ground
        float raylength = 4f; // Adjust based on character collider size
        LayerMask ground = LayerMask.GetMask("Ground"); // Ensure "Ground" layer exists
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raylength, ground);

        // Debug ray to visualize in the Scene view
        Debug.DrawRay(transform.position, Vector2.down * raylength, Color.yellow);

        // Return true if raycast hits ground
        bool answer = (hit.collider != null);
        Debug.Log("Ground Detected: " + answer);
        return answer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        playerHP -= damage;
        Debug.Log($"Player HP: {playerHP}");

        if (playerHP <= 0)
        {
            Debug.Log("Player has died!");
        }
    }

    void AttackEnemy()
    {
        // Check for enemies in front of the player
        Vector2 attackDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Direction based on facing
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);

        Debug.DrawRay(transform.position, attackDirection * attackRange, Color.red);

        //if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        //{
        //    Debug.Log("Enemy hit!");
        //    hit.collider.GetComponent<Enemy>().TakeDamage(1);
        //}
    }
}
