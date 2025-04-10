using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GunData weaponData;
    public CharacterData characterData;
    public Transform firePoint;
    public PhotonView pv;
    public Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    public Animator animator;
    public SpriteRenderer sr;
    public Slider hpSlider;
    public TMP_Text nameText;


    [Header("Movement")]
    [SerializeField] private LayerMask groundLayerMask;
    private Vector3 footPosition;
    public float speed;
    private float jumpForce = 10f;
    private int maxJumpCount = 2;
    private int currentJumpCount = 0;
    private int hp;
    private bool isGrounded;

    [Header("Gun")]
    [SerializeField] private GameObject gun;
    private float gunDamage;
    private float maxShotDelay;
    private float curShotDelay;
    private float reloadTime;
    private float effectiveRange;
    private int maxAmmoPerMag;
    private int curAmmoPerMag;
    private WaitForSeconds reloadWaitForSeconds;

    [Header("Knife and Grenade")]
    private int curGrenade;
    private float meleeDamage = 15f;
    private float maxAttackDelay = 0.4f;
    private float curAttackDelay;

    [Header("MeleeAttack")]
    [SerializeField] private Transform meleeBoxTransform;
    [SerializeField] private Vector2 meleeBoxSize;
    private bool isAttack = false;

    private KeyCode fire = KeyCode.J;
    private KeyCode meleeAttack = KeyCode.I;
    private KeyCode throwGrenade = KeyCode.U;
    private KeyCode jump = KeyCode.Space;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        //닉네임
        nameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nameText.color = pv.IsMine ? Color.blue : Color.red;
    }

    private void Start()
    {
        //총 정보 초기화
        maxShotDelay = weaponData.perShot;
        maxAmmoPerMag = weaponData.magazine;
        reloadTime = weaponData.reloadTime;
        reloadWaitForSeconds = new WaitForSeconds(reloadTime);
        curAmmoPerMag = maxAmmoPerMag;
        gunDamage = weaponData.damage;
        effectiveRange = weaponData.effectiveRange;
        curGrenade = 10;

        //캐릭터 정보 초기화
        speed = characterData.speed;
        hp = characterData.maxHP;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            curShotDelay += Time.deltaTime;
            curAttackDelay += Time.deltaTime;

            Move();
            if (Input.GetKeyDown(jump))
            {
                Jump();
            }
            Fire();
            if (curAmmoPerMag <= 0)
            {
                StartCoroutine(Reroad());
            }
            ThrowGrenade();
            MeleeAttack();
        }
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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    #region Player_Movement
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
        else if (x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }

        rb.linearVelocity = new Vector2(x * speed / 10, rb.linearVelocityY);
    }

    private void Jump()
    {
        if (currentJumpCount > 0)
        {
            currentJumpCount--;

            photonView.RPC("JumpRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void JumpRPC()
    {
        rb.linearVelocity = Vector2.up * jumpForce;
        animator.speed = 0;
    }
    #endregion

    #region Gun_Fire

    private void Fire()
    {
        if (!Input.GetKey(fire)) return;
        if (curShotDelay < maxShotDelay) return;
        if (curAmmoPerMag <= 0) return;

        // 동기화
        photonView.RPC("FireRPC", RpcTarget.All, firePoint.position, transform.rotation);

        curShotDelay = 0;
        curAmmoPerMag--;
    }

    [PunRPC]
    void FireRPC(Vector3 pos, Quaternion rot)
    {
        GameObject bullet = ObjectPool.SpawnFromPool("Bullet", pos, rot);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.InitBullet(firePoint, gunDamage, effectiveRange);
    }
    private IEnumerator Reroad()
    {
        yield return reloadWaitForSeconds;
        curAmmoPerMag = maxAmmoPerMag;
    }
    #endregion

    #region Grenade_Throw
    private void ThrowGrenade()
    {
        if (!Input.GetKeyDown(throwGrenade)) return;
        if (curGrenade <= 0) return;
        GameObject grenade = ObjectPool.SpawnFromPool("Grenade", transform.position);
        if (grenade != null)
        {
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(firePoint.right.x * 4, 7), ForceMode2D.Impulse);
        }
        curGrenade--;
    }
    #endregion

    #region Melee_Attack
    private void MeleeAttack()
    {
        if (!Input.GetKeyDown(meleeAttack)) return;
        if (curAttackDelay < maxAttackDelay) return;

        isAttack = true;

        gun.transform.rotation = Quaternion.Euler(gun.transform.rotation.x, gun.transform.rotation.y, -15f);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(meleeBoxTransform.transform.position, meleeBoxSize, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(meleeDamage);
            }
        }
        curAttackDelay = 0;

        StartCoroutine(ResetGunRotation());
    }

    private IEnumerator ResetGunRotation()
    {
        yield return new WaitForSeconds(0.3f); // 0.2초 딜레이 후 복귀
        isAttack = false;
        gun.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(meleeBoxTransform.transform.position, meleeBoxSize);
    }
    #endregion
}
