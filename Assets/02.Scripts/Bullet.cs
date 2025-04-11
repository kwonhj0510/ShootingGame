using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public PhotonView potonView;
    private Transform startPosition;

    private float speed = 16f;
    public float range;
    public float damage;

    private void Awake()
    {
        startPosition = transform;
    }
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        //if (Vector2.Distance(startPosition.transform.position, transform.position) >= range)
        //{
        //    photonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine && collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
           
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            photonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            photonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void DestroyRPC() => Destroy(gameObject);

    /// <summary>
    /// �Ѿ��� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    /// <param name="position">���� ��ġ</param>
    /// <param name="damage">������</param>
    /// <param name="range">��Ÿ�</param>

    [PunRPC]
    public void InitBulletRPC(float damage, float range)
    {
        Debug.Log("�̴ֺ�");
        this.damage = damage;
        this.range = range;
    }
}
