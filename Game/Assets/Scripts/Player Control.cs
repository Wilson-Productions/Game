using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    Transform respawnPoint;

    public int lives = 3;
    public int health = 3;
    public int maxHealth = 5;

    float inputX;
    public float jumpHeight = 5;
    public float groundDetectDistance = 1.1f;
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnPoint = GameObject.Find("RespawnPoint").transform;
        rb=GetComponent<Rigidbody2D>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if(lives > 0)
            {
                transform.position = respawnPoint.position;
                health = maxHealth;
                lives--;
            }

            if(lives <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Movement System
        Vector2 tempMove = rb.linearVelocity;

            tempMove.x = inputX * speed;

        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 InputAxis = context.ReadValue<Vector2>();

        inputX = InputAxis.x;
    }

    public void Jump()
    {
        if (Physics2D.Raycast(transform.position, -transform.up, groundDetectDistance))
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "basicenemy")
            health--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "health" && health < maxHealth)
        {
            health++;
            Destroy(collision.gameObject);
        }
    }
}