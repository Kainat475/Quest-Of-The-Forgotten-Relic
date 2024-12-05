using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene

public class PlayerLogic : MonoBehaviour
{
	// Variables for movement and jump
	public float moveSpeed = 5f;
	public float jumpForce = 10f;
	private Rigidbody2D rb;
	private bool isGrounded;
	private Animator animator;
	public Collider2D PlayerCollider;
	public AudioClip keyCollectionSound;
	public AudioClip healthplusplus;
	public AudioClip healthminusminus;

	private AudioSource audioSource;

	public LayerMask groundLayer;

	// Key counter
	private int keyCount = 0;

	// Player health
	private int health = 100; // Starting health of the player

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.freezeRotation = true;
		PlayerCollider = GetComponent<Collider2D>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		float moveX = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

		animator.SetBool("isWalking", moveX != 0);

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			animator.SetTrigger("isJumping");
		}
	}

	void FixedUpdate()
	{
		isGrounded = IsGrounded();
		animator.SetBool("isGrounded", isGrounded);

		if (isGrounded)
		{
			animator.ResetTrigger("isJumping");
		}
	}

	private bool IsGrounded()
	{
		Vector2 position = transform.position;
		Vector2 direction = Vector2.down;
		float distance = 0.1f;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
		return hit.collider != null;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("LevelEndTrigger"))
		{
			SceneManager.LoadSceneAsync(3);
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Collision detected with: " + collision.gameObject.name);

		// Handle key collection
		if (collision.CompareTag("Key"))
		{
			keyCount++;
			Debug.Log("Key collected! Total keys: " + keyCount);

			// Play key collection sound
			if (audioSource != null && keyCollectionSound != null)
			{
				audioSource.PlayOneShot(keyCollectionSound);
			}

			Destroy(collision.gameObject);
		}

		// Handle enemy collision
		if (collision.CompareTag("Enemy"))
		{
			health -= 50; // Decrease health by 50
			Debug.Log("Hit by enemy! Health: " + health);

			if (audioSource != null && healthminusminus != null)
			{
				audioSource.PlayOneShot(healthminusminus);
			}

			// Check if health is 0 or less
			if (health <= 0)
			{
				Debug.Log("Player is dead! Restarting level...");
				SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart level
			}
		}

		// Handle potion collision
		if (collision.CompareTag("Potion"))
		{
			health += 25; // Increase health by 25
			health = Mathf.Clamp(health, 0, 100); // Ensure health does not exceed 100
			Debug.Log("Potion collected! Health: " + health);
			if (audioSource != null && healthplusplus != null)
			{
				audioSource.PlayOneShot(healthplusplus);
			}
			Destroy(collision.gameObject);
		}

		// Handle level completion
		if (collision.CompareTag("EndPoint"))
		{
			if (keyCount >= 3)
			{
				Debug.Log("Level Complete! Moving to the next level...");
				LoadNextLevel();
			}
			else
			{
				Debug.Log("Collect all keys to unlock the EndPoint!");
			}
		}
	}

	// Load the next level
	private void LoadNextLevel()
	{
		// Get the current scene index and load the next scene
		//int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(1);
	}
}
