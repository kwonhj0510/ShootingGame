using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public CharacterData characterData;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;

    [SerializeField] private LayerMask groundLayerMask;

    public float speed;
    private float jumpForce = 10f;
    private int maxJumpCount = 2;
    private int currentJumpCount = 0;
    private int hp;
    private bool isGrounded;

    private Vector3 footPosition;

    private KeyCode keyCodeJump = KeyCode.Space;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //선택한 캐릭터 스탯 적용
        speed = characterData.speed;
        hp = characterData.maxHP;
    }

    private void FixedUpdate()
    {
        //바닥 확인
        Bounds bounds = capsuleCollider.bounds;
        footPosition = new Vector3(bounds.center.x, bounds.min.y);
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayerMask);

        if (isGrounded && rb.linearVelocityY < 0) 
        {
            currentJumpCount = maxJumpCount;
            animator.speed = 1;
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
        if (x == 0)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
        //Flip
        if (x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }

        rb.linearVelocity = new Vector2(x * speed / 10, rb.linearVelocityY);
    }

    private void Jump()
    {
        if (currentJumpCount > 0)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            animator.speed = 0;
            currentJumpCount--;
        }
    }
}
