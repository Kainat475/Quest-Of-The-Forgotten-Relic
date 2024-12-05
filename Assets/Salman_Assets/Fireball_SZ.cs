using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene

public class Fireball : MonoBehaviour
{
	public float moveSpeed = 2f; // Speed of fireball movement
	public float moveDistance = 3f; // Distance the fireball moves up and down
	private Vector2 startPosition; // Starting position of the fireball
	private bool movingUp = true; // Direction flag
	public GameObject player; // Reference to the player GameObject

	private Rigidbody2D rb; // Reference to the Rigidbody2D

	private void Start()
	{
		// Store the starting position of the fireball
		startPosition = transform.position;

		// Get the Rigidbody2D component
		rb = GetComponent<Rigidbody2D>();

		// Ensure Rigidbody2D is Kinematic and freeze rotation
		if (rb != null)
		{
			rb.bodyType = RigidbodyType2D.Kinematic;
			rb.freezeRotation = true; // Freeze rotation to prevent unwanted rotation
		}
	}

	private void Update()
	{
		// Move the fireball up and down
		if (movingUp)
		{
			rb.velocity = Vector2.up * moveSpeed;

			// Check if the fireball has reached the upper limit
			if (transform.position.y >= startPosition.y + moveDistance)
			{
				movingUp = false; // Switch direction
			}
		}
		else
		{
			rb.velocity = Vector2.down * moveSpeed;

			// Check if the fireball has reached the lower limit
			if (transform.position.y <= startPosition.y - moveDistance)
			{
				movingUp = true; // Switch direction
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Prevent passing through the Tilemap Collider
		if (collision.gameObject.CompareTag("Tilemap"))
		{
			// Reverse direction upon hitting the Tilemap
			movingUp = !movingUp;
			return; // Stop further collision processing
		}

		// Check if the fireball collides with the player
		if (collision.gameObject.CompareTag("Player"))
		{
			PlayDeadAnimation(collision.gameObject);

			// Restart the scene after a short delay
			Invoke(nameof(RestartScene), 3f); // Delay to allow dead animation to play
		}
	}

	// Play the dead animation on the player
	private void PlayDeadAnimation(GameObject player)
	{
		Animator playerAnimator = player.GetComponent<Animator>();

		if (playerAnimator != null)
		{
			playerAnimator.SetTrigger("isDead"); // Trigger the dead animation
		}
	}

	// Restart the current scene
	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
