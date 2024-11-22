using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float jumpForce = 5f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private Animator animator; // Referência ao Animator
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private float dashDirection;
    public float climbSpeed = 5f; // Velocidade de subida na escada
    private bool isClimbing = false; // Verifica se está na escada

    public GameObject circle;
    private Rigidbody2D circleRb;

    public GameObject circle1;
    private Rigidbody2D circleRb1;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Pega o Animator anexado ao jogador

        if (circle != null)
        {
            circleRb = circle.GetComponent<Rigidbody2D>();
        }
        if (circle1 != null)
        {
            circleRb1 = circle1.GetComponent<Rigidbody2D>();
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

        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
        }
    }

    void Move()
    {
        if (!isDashing)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            dashDirection = moveInput;

            // Verifica se está no chão antes de alterar para a animação de "andar"
            if (isGrounded)
            {
                if (moveInput != 0)
                {
                    animator.SetInteger("transition", 1); // Estado de corrida
                }
                else
                {
                    animator.SetInteger("transition", 0); // Estado parado
                }
            }

            // Inverte o jogador no eixo X se estiver indo para a esquerda
            if (moveInput < 0) // Andando para a esquerda
            {
                // Flipar no eixo X
                if (transform.localScale.x > 0) // Se o jogador não estiver flipado
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
            else if (moveInput > 0) // Andando para a direita
            {
                // Voltar ao normal
                if (transform.localScale.x < 0) // Se o jogador estiver flipado
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }



    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            audioManager.PlaySFX(audioManager.jump);

            // Atualiza a animação para "Jump"
            animator.SetInteger("transition", 2);
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
            float time = Time.time;
            lastDashTime = time;

            // Atualiza a animação para "Dash"
            animator.SetInteger("transition", 4);

            Invoke("EndDash", dashDuration);
        }
    }

    void EndDash()
    {
        isDashing = false;
        canDash = true;

        // Retorna à animação padrão
        if (isGrounded)
        {
            animator.SetInteger("transition", 0); // Parado
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            audioManager.PlaySFX(audioManager.walk);

            // Retorna à animação padrão quando estiver no chão
            if (!isDashing)
            {
                animator.SetInteger("transition", 0);
            }
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
        if (collider.CompareTag("Obs2"))
        {
            Debug.Log("Você entrou na área do Obs2!");

            if (circleRb1 != null)
            {
                circleRb1.isKinematic = false;
                Debug.Log("Rigidbody2D do Circle ativado!");
            }
        }
        // O OBJETO DO ESPINHO TEM Q TER UM COLLIDER E IS TRIGGER ATIVADO NO COLLIDER!!!!, LEMBRAR DE COLOCAR O OBJETO DO METEORO QUE VAI CAIR E TAL
        if (collider.CompareTag("die"))
        {
            Debug.Log("Player colidiu com um objeto 'Die'. Reposicionando...");
            animator.SetInteger("transition", 3);
            Invoke("ResetPlayerPosition", 0.4f);
            audioManager.PlaySFX(audioManager.death);

        }

        if (collider.CompareTag("escada"))
        {
            isClimbing = true; // Ativa a subida
            rb.gravityScale = 0; // Remove o efeito da gravidade enquanto sobe
            Debug.Log("Escada");
        }

        if (collider.CompareTag("fim"))
        {
            SceneManager.LoadSceneAsync(2);
        }
    }

    // Detecta quando sai da escada
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("escada"))
        {
            isClimbing = false; // Para a subida
            rb.gravityScale = 1; // Restaura a gravidade
        }
    }

    void ResetPlayerPosition()
    {
        // Reposiciona o jogador para o ponto inicial
        transform.position = new Vector2(0, 2);

        if (circle != null)
        {
            circle.transform.position = new Vector2(16.7f, 23.5f);
            circleRb.velocity = Vector2.zero;         
            circleRb.angularVelocity = 0f;
            circleRb.isKinematic = true;
        }
        if (circle1 != null)
        {
            circle1.transform.position = new Vector2(76.47f, 46.73f);
            circleRb1.velocity = Vector2.zero;
            circleRb1.angularVelocity = 0f;
            circleRb1.isKinematic = true;
        }

        // Retorna a animação para o estado padrão
        animator.SetInteger("transition", 0);
    }
}
