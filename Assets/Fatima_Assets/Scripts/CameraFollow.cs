using UnityEngine;
using System.Collections;

public class CameraFollow: MonoBehaviour
{
	public Transform player;   // Reference to the player's Transform
	public float smoothSpeed = 0.125f; // Smoothing speed for smooth camera movement
	public float initialWaitTime = 2f; // Time to wait at the initial position

	private bool followPlayer = false; // Flag to start following the player

	void Start()
	{
		// Start the coroutine to handle the camera's initial movement
		StartCoroutine(InitialCameraMovement());
	}

	void LateUpdate()
	{
		// Follow the player only after the initial wait
		if (followPlayer)
		{
			// Check if the player has moved to the right of the camera
			if (player.position.x > transform.position.x)
			{
				// Move the camera to the right to follow the player
				Vector3 desiredPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

				// Smoothly interpolate between the current and desired position
				Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

				// Update the camera's position
				transform.position = smoothedPosition;
			}
		}
	}

	IEnumerator InitialCameraMovement()
	{
		// Move the camera to the initial position and wait
		transform.position = new Vector3(-15.93f, transform.position.y, transform.position.z);
		yield return new WaitForSeconds(initialWaitTime);

		// Move the camera to the next position
		transform.position = new Vector3(0.41f, transform.position.y, transform.position.z);

		// Start following the player
		followPlayer = true;
	}
}
