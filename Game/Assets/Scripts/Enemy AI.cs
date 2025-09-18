using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody2D enemyRB;

    public float edgeCheckDistance = 1.0f;
    public float speed = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tempSpeed = enemyRB.linearVelocity;

        if (!Physics2D.Raycast(transform.position, -transform.up, edgeCheckDistance))
        {
            speed *= -1;
        }

        tempSpeed.x = speed;

        enemyRB.linearVelocity = tempSpeed;
    }
}
