using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
