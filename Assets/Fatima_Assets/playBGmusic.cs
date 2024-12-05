using UnityEngine;



public class playBGmusic : MonoBehaviour
{
	public AudioClip backgroundMusic; // Assign your background music clip in the Inspector
	private AudioSource audioSource;

	private void Awake()
	{

		// Initialize AudioSource and play the background music
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		audioSource.clip = backgroundMusic;
		audioSource.loop = true;
		audioSource.playOnAwake = true;
		audioSource.volume = 0.5f; // Adjust volume as needed
		audioSource.Play();
	}
}
