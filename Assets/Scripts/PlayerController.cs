using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float jumpForce = 5f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private float dashDirection;

    public GameObject circle;
    private Rigidbody2D circleRb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (circle != null)
        {
            circleRb = circle.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        Move();
        Jump();

        if (!isDashing)
        {
            Dash();
        }
    }

    void Move()
    {
        if (!isDashing)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            dashDirection = moveInput;
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Time.time >= lastDashTime + dashCooldown)
        {
            if (dashDirection == 0)
                dashDirection = transform.localScale.x > 0 ? 1 : -1;

            rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

            canDash = false;
            isDashing = true;
            lastDashTime = Time.time;

            Invoke("EndDash", dashDuration);
        }
    }

    void EndDash()
    {
        isDashing = false;
        canDash = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Obs1"))
        {
            Debug.Log("Você entrou na área do Obs1!");

            if (circleRb != null)
            {
                circleRb.isKinematic = false; 
                Debug.Log("Rigidbody2D do Circle ativado!");
            }
        }
    }
}
