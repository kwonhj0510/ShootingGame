using UnityEngine;

public class Bullet : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(1); //Á×¾úÀ» ¶© µ¥¹ÌÁö°¡ µé¾î°¡¸é ¾ÈµÊ
            ObjectPool.ReturnToPool("Bullet", this.gameObject);
        }
    }
}
