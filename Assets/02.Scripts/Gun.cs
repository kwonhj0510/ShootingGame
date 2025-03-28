using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public WeaponData weaponData;

    private float maxShotDelay;
    private float curShotDelay;
    private float reloadTime;
    private float bulletSpeed = 15;
    private int maxAmmoPerMag;
    private int curAmmoPerMag = 0;

    private KeyCode keyCodeFire = KeyCode.J;

    private void Start()
    {
        maxShotDelay = weaponData.perShot;
        maxAmmoPerMag = weaponData.magazine;
        reloadTime = weaponData.reloadTime;
    }

    private void Update()
    {
        curShotDelay += Time.deltaTime;
        Fire();
        if(curAmmoPerMag == maxAmmoPerMag)
        {
            Invoke("Reroad", reloadTime);
        }
    }

    private void Fire()
    {
        if (!Input.GetKey(keyCodeFire)) return;
        if (curShotDelay < maxShotDelay) return;
        if (curAmmoPerMag == maxAmmoPerMag) return;
        GameObject bullet = ObjectPool.SpawnFromPool("Bullet", firePoint.position, transform.rotation);
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);
            curShotDelay = 0;
            curAmmoPerMag += 1;
        }
    }

    private void Reroad()
    {
        curAmmoPerMag = 0;
    }
}
