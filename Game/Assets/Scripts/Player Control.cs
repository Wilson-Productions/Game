using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public SpriteRenderer WpnAnim;
    public bool grounded;
    public Animator Anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponOffset = weapon.transform.localPosition;

        respawnPoint = GameObject.Find("RespawnPoint").transform;
        rb = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0).gameObject;
        WpnAnim = weapon.GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = (Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0f, -Vector2.up, groundDetectDistance));
        Anim.SetBool("AnimFloored", grounded);
        if (grounded && inputX != 0f)
            {
            Anim.SetBool("Tralalalala", true);
            Anim.SetFloat("WhichWay", inputX);
        }
        else
        {
            Anim.SetBool("Tralalalala", false);
            Anim.SetFloat("WhichWay", 0);
        }
        //if position
            ;
        if (health <= 0)
        {
            if (lives > 0)
            {
                transform.position = respawnPoint.position;
                health = maxHealth;
                lives--;
            }

            if (lives <= 0)
                SceneManager.LoadScene(1);
        }

        // Movement System
        Vector2 tempMove = rb.linearVelocity;

        tempMove.x = inputX * speed;

        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up);
        Vector2 directionModifier = new Vector2(weaponOffset.x * inputX, weaponOffset.y);
        if (inputX != 0f)
            weapon.transform.localPosition = directionModifier;
        if (inputX < 0f)
            WpnAnim.flipX = true;
        if (inputX > 0f)
            WpnAnim.flipX = false;
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
        if (grounded)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "health" && health < maxHealth)
        {
            health++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}