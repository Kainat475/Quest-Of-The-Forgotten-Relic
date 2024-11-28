using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumblingWeed : MonoBehaviour
{
    public float moveSpeed = 5f;     // Speed at which the tumbleweed moves horizontally
    public float tumbleSpeed = 700f; // Speed of the tumbling rotation
    public float fallSpeed = 2f;

    public Vector2 fallPoint;

    private Vector3 initialPosition;
    [SerializeField] private bool isFalling;

    private void Start()
    {
        initialPosition = transform.position;
        isFalling = false;
    }

    void Update()
    {
        if (!isFalling)
        {
            // Move the tumbleweed to the right
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.World);

            // Rotate the tumbleweed for the tumbling effect
            transform.Rotate(0, 0, -tumbleSpeed * Time.deltaTime);

            // Check if the tumbleweed has reached the x-axis limit
            if (transform.position.x >= fallPoint.x)
            {
                isFalling = true; // Start falling
            }
        }
        else
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime, Space.World);
            transform.Rotate(0, 0, -tumbleSpeed * Time.deltaTime);
            if (transform.position.y <= fallPoint.y)
            {
                isFalling = false;
                transform.position = initialPosition;

                //transform.Translate(initialPosition);
            }
        }
    }
}
