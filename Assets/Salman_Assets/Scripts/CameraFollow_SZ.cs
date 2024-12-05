using UnityEngine;
using System.Collections; // Added namespace for IEnumerator

public class CameraFollowSZ : MonoBehaviour
{
    public Transform player;   // Reference to the player's Transform
    public float smoothSpeed = 0.125f; // Smoothing speed for smooth camera movement

    private bool cameraInitialized = false; // Flag to track if the initial movement is complete

    void Start()
    {
        // Start the coroutine to move the camera from -16.79 to -0.41 over 2 seconds
        StartCoroutine(MoveCameraToInitialPosition());
    }

    void LateUpdate()
    {
        // After the initial movement, follow the player
        if (cameraInitialized && player.position.x > transform.position.x)
        {
            // Move the camera to the right to follow the player
            Vector3 desiredPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

            // Smoothly interpolate between the current and desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }

    private IEnumerator MoveCameraToInitialPosition()
    {
        // Get the initial and target positions
        Vector3 initialPosition = new Vector3(-16.79f, transform.position.y, transform.position.z);
        Vector3 targetPosition = new Vector3(-0.41f, transform.position.y, transform.position.z);

        // Set the camera's initial position
        transform.position = initialPosition;

        float elapsedTime = 0f;
        float duration = 2f; // Time in seconds to move the camera

        // Smoothly move the camera to the target position over 2 seconds
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set correctly
        transform.position = targetPosition;

        // Mark the initialization as complete
        cameraInitialized = true;
    }
}
