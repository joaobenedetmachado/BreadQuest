using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; //velocidades basica e tal
    public float dashSpeed = 15f;
    public float jumpForce = 5f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb; // define os componentes e tal, e pega a bool de dar dash, e o tempo, e a direcao do dash
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private float dashDirection;

    void Start() // inicia o bgl do rigidibody
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    { // inicia a func do move e jump
        Move();
        Jump();

        if (!isDashing) // e so pode dar dash se ele nao tiver no dah
        {
            Dash();
        }
    }

    void Move() // pra se mover normal
    {
        if (!isDashing)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            dashDirection = moveInput;
        }
    }

    void Jump() // jump
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void Dash() //dash no Left Shigt
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Time.time >= lastDashTime + dashCooldown)
        {
            // garante que o dashDirection seja -1 ou 1, ou permanece na ultima direcao usada
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
}