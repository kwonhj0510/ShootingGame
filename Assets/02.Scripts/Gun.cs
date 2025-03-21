using UnityEngine;

public class Gun : MonoBehaviour
{
    private KeyCode keyCodeFire = KeyCode.J;
    public Transform firePoint;

    private void Update()
    {
        if (Input.GetKeyDown(keyCodeFire))
        {
            GameObject bullet = ObjectPool.SpawnFromPool("Bullet", firePoint.position, transform.rotation);
            if(bullet != null)
            {
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.SetDirection(firePoint.right);
            }
        }
    }
}
