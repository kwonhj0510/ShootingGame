using UnityEngine;

public class Bullet : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(1);
            ObjectPool.ReturnToPool("Bullet", this.gameObject);
        }
    }
}
