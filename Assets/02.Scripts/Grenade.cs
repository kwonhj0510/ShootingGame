using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody2D rb;

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

        //수류탄 터지는 코드
        ObjectPool.ReturnToPool("Grenade", this.gameObject);
    }
}
