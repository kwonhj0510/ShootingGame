using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 direction = Vector3.right;

    //private void OnDisable()
    //{
    //    Debug.Log("¾Ó");
    //    ObjectPool.ReturnToPool("Bullet", this.gameObject);
    //}

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized; 
    }
}
