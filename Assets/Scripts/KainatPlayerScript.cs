using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Rigidbody2D rb_player;
    [SerializeField] private Vector3 playerScale; // Scale of the player sprite
    [SerializeField] private float playerMove;

    [Header("Player Run Settings")]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;

    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGround;       // Flag to track if character is on the ground

    // Start is called before the first frame update
    void Start()
    {
        playerScale = transform.localScale;
        defaultSpeed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Taking Player Movement Input (left, right) (a, d)
        playerMove = Input.GetAxis("Horizontal");

        // Run and Normal Walking Functionality
        if (Input.GetKey(KeyCode.LeftShift))       
            playerSpeed = sprintSpeed;
        else
            playerSpeed = defaultSpeed;

        // Flip character sprite based on movement direction
        if (playerMove > 0)
        {
            // left direction facing
            transform.localScale = new Vector3(Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
        }
        else if (playerMove < 0)
        {
            // right direction facing
            transform.localScale = new Vector3(-Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
        }

        // Jumping Functionality
        isGround = isGrounded();

        // Jump when Space is pressed and character is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Debug.Log("Player jumped!");
            isJumping = true;
            rb_player.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //transform.position += new Vector3(0, jumpForce * Time.deltaTime, 0);
        }

        // Stop jump when Space is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    // Dealing with Player Movement in this function
    void FixedUpdate()
    {
        // Apply horizontal movement
        rb_player.velocity = new Vector2(playerMove * playerSpeed, rb_player.velocity.y);
        Debug.Log("Applied Velocity: " +  rb_player.velocity);
    }

    bool isGrounded()
    {
        // Cast a ray downwards to check for ground
        float raylength = 1.5f; // Adjust based on character collider size
        LayerMask ground = LayerMask.GetMask("Ground"); // Ensure "Ground" layer exists
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raylength, ground);

        // Debug ray to visualize in the Scene view
        Debug.DrawRay(transform.position, Vector2.down * raylength, Color.yellow);

        // Return true if raycast hits ground
        bool answer = (hit.collider != null);
        Debug.Log("Ground Detected: " + answer);
        return answer;
    }
}
