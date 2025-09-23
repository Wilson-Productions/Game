using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    Transform respawnPoint;
    public GameObject weapon;
    public int lives = 3;
    public int health = 3;
    public int maxHealth = 5;
    public Vector2 weaponOffset;
    float inputX;
    public float jumpHeight = 5;
    public float groundDetectDistance = 1.1f;
    public float speed = 5f;
    public float attackDuration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponOffset = weapon.transform.localPosition;

        respawnPoint = GameObject.Find("RespawnPoint").transform;
        rb = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0).gameObject;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (lives > 0)
            {
                transform.position = respawnPoint.position;
                health = maxHealth;
                lives--;
            }

            if (lives <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Movement System
        Vector2 tempMove = rb.linearVelocity;

        tempMove.x = inputX * speed;

        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up);
        Vector2 directionModifier = new Vector2(weaponOffset.x * inputX, weaponOffset.y);
        if(inputX != 0f)
            weapon.transform.localPosition = directionModifier;
    }

    // Create a function that spawns an attack zone in front of the player
    // Orient the direction of the attack zone in the player's facing direction (usually transform.right)


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
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weapon.SetActive(true);
            StartCoroutine(AttackDuration());
        }

    }
    IEnumerator AttackDuration() {
        yield return new WaitForSeconds(attackDuration);
        weapon.SetActive(false);
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