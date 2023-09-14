using UnityEngine;

public class Wander : MonoBehaviour
{
    public float moveDistance = 20f; // Distance to move left or right
    public float moveSpeed = 1f; // Speed of movement

    private Vector3 startPos;
    private bool movingRight = true;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.position.x >= startPos.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x <= startPos.x)
            {
                movingRight = true;
            }
        }
    }
}
