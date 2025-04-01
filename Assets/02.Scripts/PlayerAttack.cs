using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint;
    public GunData weaponData;

    //ÃÑ Á¤º¸
    private float gunDamage;
    private float maxShotDelay;
    private float curShotDelay;
    private float reloadTime;
    private float effectiveRange;
    private int maxAmmoPerMag;
    private int curAmmoPerMag;

    //Ä® ¹× ¼ö·ùÅº
    private int curGrenade;
    private float meleeDamage = 15f;
    private float maxAttackDelay = 0.4f;
    private float curAttackDelay;

    private bool isAttack = false;

    private Vector2 meleeBoxPosition;
    private Vector2 meleeBoxSize;

    private KeyCode fire = KeyCode.J;
    private KeyCode meleeAttack = KeyCode.I;
    private KeyCode throwGrenade = KeyCode.U;

    private void Start()
    {
        maxShotDelay = weaponData.perShot;
        maxAmmoPerMag = weaponData.magazine;
        reloadTime = weaponData.reloadTime;
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
            Invoke("Reroad", reloadTime);
        }
    }

    private void Fire()
    {
        if (!Input.GetKey(fire)) return;
        if (curShotDelay < maxShotDelay) return;
        if (curAmmoPerMag <= 0) return;

        GameObject bullet = ObjectPool.SpawnFromPool("Bullet", firePoint.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.range = effectiveRange;

        curShotDelay = 0;
        curAmmoPerMag--;
    }

    private void Reroad()
    {
        curAmmoPerMag = maxAmmoPerMag;
    }

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
        if(!Input.GetKeyDown(meleeAttack) || isAttack) return;
        if (curAttackDelay < maxAttackDelay) return;

        meleeBoxPosition = new Vector2(transform.position.x + 0.5f, transform.position.y);
        meleeBoxSize = new Vector2(2f, 1f);

        isAttack = true;

        Collider2D[] colliders =  Physics2D.OverlapBoxAll(meleeBoxPosition, meleeBoxSize, 0f);
        foreach(Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(meleeDamage);
            }
        }
        curAttackDelay = 0;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;

    //    if (isAttack)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawCube(meleeBoxPosition, meleeBoxSize);
    //    }
    //}
}
