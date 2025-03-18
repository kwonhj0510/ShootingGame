using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterData characterData;

    private float jumpForce = 10f;
    private float speed;
    private int hp;

    private Rigidbody2D rb;

    private KeyCode keyCodeJump = KeyCode.Space;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = characterData.speed;
        hp = characterData.maxHP;
    }

    private void Update()
    {
        Move();
        if (Input.GetKeyDown(keyCodeJump))
        {
            Jump();
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal"); 

        rb.linearVelocity = new Vector2(x * speed / 10, rb.linearVelocityY);
    }

    private void Jump()
    {
        rb.linearVelocity = Vector2.up * jumpForce;
    }
}
