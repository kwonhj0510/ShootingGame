using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 explosionRadius = new Vector2(3f, 3f);
    private bool isBomb = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector3.zero;
            StartCoroutine(ExplodeGrenade());
        }
    }

    private IEnumerator ExplodeGrenade()
    {
        yield return new WaitForSeconds(1);
        isBomb = true;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, explosionRadius, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(enemy.maxHp);
            }
        }

        yield return new WaitForSeconds(1);
        isBomb = false;
        //수류탄 터지는 코드
        ObjectPool.ReturnToPool("Grenade", this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (isBomb)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, explosionRadius);
        }
    }
}
