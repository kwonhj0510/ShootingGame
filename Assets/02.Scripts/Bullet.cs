using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 16f;
    public float range;
    public float damage;

    private Transform startPosition;
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (Vector2.Distance(startPosition.transform.position, transform.position) >= range)
        {
            ObjectPool.ReturnToPool("Bullet", this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            ObjectPool.ReturnToPool("Bullet", this.gameObject);
        }
    }

    /// <summary>
    /// �Ѿ��� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    /// <param name="position">���� ��ġ</param>
    /// <param name="damage">������</param>
    /// <param name="range">��Ÿ�</param>
    public void InitBullet(Transform position, float damage, float range)
    {
        startPosition = position;
        this.damage = damage;
        this.range = range;
    }
}
