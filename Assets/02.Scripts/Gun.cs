using UnityEngine;

public class Gun : MonoBehaviour
{
    private KeyCode keyCodeFire = KeyCode.J;

    private void Update()
    {
        if (Input.GetKey(keyCodeFire))
        {
            ObjectPool.SpawnFromPool("Bullet", transform.position);
        }
    }
}
