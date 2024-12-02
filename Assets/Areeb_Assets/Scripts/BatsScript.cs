using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BatsScript : MonoBehaviour
{
	public Vector3 playerscale;

	public GameObject left;
	public GameObject right;
	public GameObject top;
	public GameObject bottom;
	Transform currentposition;
	Transform archposition;
	public float speed = 5f;
	public float archspeed;
	public Rigidbody2D rb;

	public GameObject Player;
	public GameObject bat;

	public Collider2D batCollider;

	public float rayLength = 10f;     
	public float diveSpeed = 10f;      
	public LayerMask playerLayer;     

	private bool isDiving = false;    
	private Transform target;
	public GameObject diveLimit;

	public float cooldownTime = 10f;
	public float coolTimer;

	// Start is called before the first frame update
	void Start()
	{
		coolTimer = cooldownTime;
		currentposition = right.transform;
		archposition = top.transform;
		playerscale = transform.localScale;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isDiving)
		{
			Vector2 horizontalDirection = (currentposition.position - transform.position).normalized;

			rb.velocity = horizontalDirection * speed;
			if (Vector2.Distance(currentposition.position, transform.position) < 0.5f)
			{
				if (currentposition == right.transform)
				{
					currentposition = left.transform;
					transform.localScale = new Vector3(-Mathf.Abs(playerscale.x), playerscale.y, playerscale.z);
				}
				else if (currentposition == left.transform)
				{
					currentposition = right.transform;
					transform.localScale = new Vector3(Mathf.Abs(playerscale.x), playerscale.y, playerscale.z);
				}
			}
		}

        coolTimer -= Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, playerLayer);

		Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red);

			if (hit.collider != null && !isDiving)
			{
				isDiving = true;
				target = hit.collider.transform;
			}

			// Dive towards the target if detected
			if (isDiving && target != null)
			{
				DiveTowardsTarget();
                coolTimer = cooldownTime;
            }
	}

	void DiveTowardsTarget()
	{
		Vector2 direction = (target.position - transform.position).normalized;
		
			transform.position = Vector2.MoveTowards(transform.position, target.position, diveSpeed * Time.deltaTime);
		if (Vector2.Distance(right.transform.position, transform.position) < 0.5f || Vector2.Distance(left.transform.position, transform.position) < 0.5f)
		{
			isDiving = false;
			target = null;
            Vector2 horizontalDirection = currentposition.position - transform.position;

            rb.velocity = horizontalDirection * speed;
        }

		else if ((Vector2.Distance(transform.position, target.position) < 1f))
		{
			isDiving = false;
			target = null;

            Vector2 horizontalDirection = (currentposition.position - transform.position).normalized;
            rb.velocity = horizontalDirection * speed;
        }
	}
}
