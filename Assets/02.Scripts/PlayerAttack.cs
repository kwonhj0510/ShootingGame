using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint;
    public Transform meleeBoxPoint;
    public WeaponData weaponData;

    private float maxAttackDelay;
    private float curAttackDelay;
    private float reloadTime;
    private float bulletSpeed = 15;
    private float damage;
    private int maxAmmoPerMag;
    private int curAmmoPerMag;
    private int curGrenade;

    private Vector2 meleeBoxPosition;
    private Vector2 meleeBoxSize;

    private KeyCode fire = KeyCode.J;
    private KeyCode meleeAttack = KeyCode.I;
    private KeyCode throwGrenade = KeyCode.U;

    private void Start()
    {
        maxAttackDelay = weaponData.perShot;
        maxAmmoPerMag = weaponData.magazine;
        reloadTime = weaponData.reloadTime;
        curAmmoPerMag = maxAmmoPerMag;
        damage = weaponData.damage;
        curGrenade = 10;
    }

    private void Update()
    {
        curAttackDelay += Time.deltaTime;
        Fire();
        ThrowGrenade();
        if (curAmmoPerMag <= 0)
        {
            Invoke("Reroad", reloadTime);
        }
    }

    private void Fire()
    {
        if (!Input.GetKey(fire)) return;
        if (curAttackDelay < maxAttackDelay) return;
        if (curAmmoPerMag <= 0) return;
        GameObject bullet = ObjectPool.SpawnFromPool("Bullet", firePoint.position, transform.rotation);
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);
            curAttackDelay = 0;
            curAmmoPerMag--;
        }
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
        Debug.Log(grenade);
        if (grenade != null)
        {
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(firePoint.right.x * 4, 7), ForceMode2D.Impulse);

            curGrenade--;
        }
    }

    private void MeleeAttack()
    {        
        meleeBoxSize = new Vector2(2f, 1f);
        if(!Input.GetKeyDown(meleeAttack)) return;
        if(curAttackDelay < maxAttackDelay) return;

        Collider2D[] colliders =  Physics2D.OverlapBoxAll(meleeBoxPosition, meleeBoxSize, 0f);
        
    }
}
