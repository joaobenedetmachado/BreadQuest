using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 10f;  
    public float jumpForce = 5f; 

    private Rigidbody2D rb;       
    private bool isGrounded;   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f);
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed; 
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else {
            // Debug.log("PERsonagem no chao");
        }
    }
}
