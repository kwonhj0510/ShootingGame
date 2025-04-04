using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint;
    public GunData weaponData;

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

    private void Start()
    {
        maxShotDelay = weaponData.perShot;
        maxAmmoPerMag = weaponData.magazine;
        reloadTime = weaponData.reloadTime;
        reloadWaitForSeconds = new WaitForSeconds(reloadTime);
        curAmmoPerMag = maxAmmoPerMag;
        gunDamage = weaponData.damage;
        effectiveRange = weaponData.effectiveRange;
        curGrenade = 10;
    }

    private void Update()
    {
        curShotDelay += Time.deltaTime;
        curAttackDelay += Time.deltaTime;

        Fire();
        ThrowGrenade();
        MeleeAttack();
        if (curAmmoPerMag <= 0)
        {
            StartCoroutine(Reroad());
        }
    }

    #region Gun_Fire
    private void Fire()
    {
        if (!Input.GetKey(fire)) return;
        if (curShotDelay < maxShotDelay) return;
        if (curAmmoPerMag <= 0) return;

        GameObject bullet = ObjectPool.SpawnFromPool("Bullet", firePoint.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.InitBullet(firePoint, gunDamage, effectiveRange);

        curShotDelay = 0;
        curAmmoPerMag--;
    }

    private IEnumerator Reroad()
    {
        yield return reloadWaitForSeconds;
        curAmmoPerMag = maxAmmoPerMag;
    }
    #endregion

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
        yield return new WaitForSeconds(0.3f); // 0.2√  µÙ∑π¿Ã »ƒ ∫π±Õ
        isAttack = false;
        gun.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(meleeBoxTransform.transform.position, meleeBoxSize);
    }
}
