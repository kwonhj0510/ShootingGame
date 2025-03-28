using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint;
    public WeaponData weaponData;

    private float maxShotDelay;
    private float curShotDelay;
    private float reloadTime;
    private float bulletSpeed = 15;
    private int maxAmmoPerMag;
    private int curAmmoPerMag;
    private int curGrenade;

    private KeyCode fire = KeyCode.J;
    private KeyCode throwGrenade = KeyCode.U;

    private void Start()
    {
        maxShotDelay = weaponData.perShot;
        maxAmmoPerMag = weaponData.magazine;
        reloadTime = weaponData.reloadTime;
        curAmmoPerMag = maxAmmoPerMag;
        curGrenade = 10;
    }

    private void Update()
    {
        curShotDelay += Time.deltaTime;
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
        if (curShotDelay < maxShotDelay) return;
        if (curAmmoPerMag <= 0) return;
        GameObject bullet = ObjectPool.SpawnFromPool("Bullet", firePoint.position, transform.rotation);
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);
            curShotDelay = 0;
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
            //rb.AddForce(new Vector2(firePoint.position.x * 4, 6), ForceMode2D.Impulse); 수류탄 포물선
            curGrenade--;
        }
    }
}
