using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class IceSpikes : MonoBehaviour
{
	public float randomTime;
	public float timer;
	public float fallSpeed = 0.5f;
	public Rigidbody2D rb;
	public Vector2 v2;
	public bool falling = false;
	public GameObject player;
	public GameObject ice;
	// Start is called before the first frame update
	void Start()
	{
		timer = randomTime;
		v2 = transform.position;
	}

	// Update is called once per frame
	void Update()
	{

		if (!falling)
		{
			transform.position = v2;
            GetComponent<Renderer>().enabled = true;
            //Debug.Log("smth4");
            timer = timer - Time.deltaTime;
		}

		if (timer <= 0)
		{
			//Debug.Log("smth1");
			falling = true;
			timer = randomTime;
		}

		if (falling)
		{
			//Debug.Log("smth2");

			transform.position += Vector3.down * fallSpeed * Time.deltaTime;

			if (transform.position.y <= player.transform.position.y)
            {
                GetComponent<Renderer>().enabled = false;
                //Debug.Log("smth3");
                falling = false;
			}

		}


	}

	public void fallingSpike()
	{
		// Make the object fall

		
	}
}
