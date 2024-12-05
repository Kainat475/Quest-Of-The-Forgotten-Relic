using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArdenMechanics : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Rigidbody2D rb_player;
    [SerializeField] private Collider2D collider_player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Vector3 playerScale;
    [SerializeField] private float playerMove;
    [SerializeField] private SpriteRenderer playerRenderer;

    [Header("Player Speed Settings")]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float pushForce = 3f;
    [SerializeField] private float speedThreshold = 0.05f;


    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGrounded;




    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        collider_player = GetComponent<Collider2D>();

        playerScale = transform.localScale;
        defaultSpeed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Taking Player Movement Input (left, right) (a, d)
        playerMove = Input.GetAxis("Horizontal");

        // Set the isRunning trigger based on horizontal input
        if (playerMove != 0)
        {
            playerAnimator.SetTrigger("isRunning");
        }
        else
        {
            playerAnimator.ResetTrigger("isRunning");
        }

        // Jumping Functionality
        isGrounded = isGroundeded();

        // Jump allwd only when Space is pressed and character is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerAnimator.SetTrigger("jumpPressed");
            isJumping = true;
            rb_player.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    // Dealing with Player Movement in this function
    void FixedUpdate()
    {
        // Applying horizontal (left, right) movement
        rb_player.velocity = new Vector2(playerMove * playerSpeed, rb_player.velocity.y);
        //Debug.Log("Applied Velocity: " +  rb_player.velocity);
    }

    bool isGroundeded()
    {
        // Cast a ray downwards to check for ground
        float raylength = 2f; // Adjust based on character collider size
        LayerMask ground = LayerMask.GetMask("Ground"); // Ensure "Ground" layer exists
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raylength, ground);

        // Debug ray to visualize in the Scene view
        Debug.DrawRay(transform.position, Vector2.down * raylength, Color.yellow);

        // Return true if raycast hits ground
        bool answer = (hit.collider != null);
        //Debug.Log("Ground Detected: " + answer);
        return answer;
    }
}
