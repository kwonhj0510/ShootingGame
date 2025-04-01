using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 16f;
    public float range;

    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (Vector2.Distance(startPosition, transform.position) >= range)
        {
            ObjectPool.ReturnToPool("Bullet", this.gameObject);
        }
    }
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
