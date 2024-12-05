using UnityEngine;

public class PlayerLogicSSZ : MonoBehaviour
{
	// Variables for movement and jump
	public float moveSpeed = 5f;  // Speed for horizontal movement
	public float jumpForce = 10f; // Force applied when jumping
	private Rigidbody2D rb;       // Reference to Rigidbody2D
	private bool isGrounded;      // Check if the player is on the ground
	private Animator animator;    // Reference to the Animator
	public Collider2D PlayerCollider;

	// LayerMask to detect ground collision
	public LayerMask groundLayer;

	// Start is called once before the first frame update
	void Start()
	{
		// Get the Rigidbody2D component attached to the player
		rb = GetComponent<Rigidbody2D>();
		// Get the Animator component
		animator = GetComponent<Animator>();
		rb.freezeRotation = true;
		PlayerCollider = GetComponent<Collider2D>();

	}

	// Update is called once per frame
	void Update()
	{
		// Horizontal movement (move right)
		float moveX = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

		// Update walking animation
		animator.SetBool("isWalking", moveX != 0);

		// Jump logic
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);

			// Play jump animation
			animator.SetTrigger("isJumping");
		}
	}

	// FixedUpdate for physics-based checks
	void FixedUpdate()
	{
		// Check if the player is grounded
		isGrounded = IsGrounded();

		// Update grounded state in the animator
		animator.SetBool("isGrounded", isGrounded);

		// Reset jump trigger if player is grounded
		if (isGrounded)
		{
			animator.ResetTrigger("isJumping");
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("isJumping");
		}
	}

	// Method to check if the player is on the ground
	private bool IsGrounded()
	{
		// Use a small raycast downwards to check for ground
		Vector2 position = transform.position;
		Vector2 direction = Vector2.down;
		float distance = 0.1f;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
		return hit.collider != null;
	}


}
