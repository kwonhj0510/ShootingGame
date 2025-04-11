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
    /// 총알을 초기화 하는 함수
    /// </summary>
    /// <param name="position">시작 위치</param>
    /// <param name="damage">데미지</param>
    /// <param name="range">사거리</param>

    [PunRPC]
    public void InitBulletRPC(float damage, float range)
    {
        Debug.Log("이닛블렉");
        this.damage = damage;
        this.range = range;
    }
}
