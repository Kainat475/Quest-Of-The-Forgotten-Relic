using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene

public class EnemyScriptSZ : MonoBehaviour
{
	public float moveSpeed = 2f; // Speed of enemy movement
	public float moveDistance = 3f; // Distance enemy moves in one direction
	private Vector2 startPosition; // Starting position of the enemy
	private bool movingRight = true; // Direction flag

	private void Start()
	{
		// Store the starting position of the enemy
		startPosition = transform.position;
	}

	private void Update()
	{
		// Move the enemy left and right
		if (movingRight)
		{
			transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

			// Check if the enemy has reached the right limit
			if (transform.position.x >= startPosition.x + moveDistance)
			{
				movingRight = false; // Switch direction
			}
		}
		else
		{
			transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

			// Check if the enemy has reached the left limit
			if (transform.position.x <= startPosition.x - moveDistance)
			{
				movingRight = true; // Switch direction
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Check if the collision is with the player
		
			// Check if the collision is with the top collider
			if (collision.collider.CompareTag("TopColliderTag"))
			{
				// Destroy the enemy if the top collider is hit
				Destroy(gameObject);
			}
			else if (collision.collider.CompareTag("SideColliderTag"))
			{
				// Play the dead animation and restart the scene
				PlayDeadAnimation(collision.gameObject);
				Invoke(nameof(RestartScene), 1f); // Delay to allow dead animation to play
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
