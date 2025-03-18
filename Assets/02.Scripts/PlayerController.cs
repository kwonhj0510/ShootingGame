using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterData characterData;

    private Rigidbody2D rb;
    private float jumpForce = 10f;
    private float speed;
    private int hp;

    [SerializeField] private LayerMask groundLayerMask;
    private CapsuleCollider2D capsuleCollider;
    private bool isGrounded;
    private Vector3 footPosition;

    private int maxJumpCount = 2;
    private int currentJumpCount = 0;

    private KeyCode keyCodeJump = KeyCode.Space;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        speed = characterData.speed;
        hp = characterData.maxHP;
    }

    private void FixedUpdate()
    {
        Bounds bounds = capsuleCollider.bounds;
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayerMask);

        if (isGrounded && rb.linearVelocityY < 0) 
        {
            currentJumpCount = maxJumpCount;
        }
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
        if (currentJumpCount > 0)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            currentJumpCount--;
        }
    }
}
