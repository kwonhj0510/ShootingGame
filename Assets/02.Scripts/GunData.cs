using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class GunData : ScriptableObject
{
    public string name;       //이름
    public float damage;              //데미지
    public int magazine;            //탄창 수
    public float effectiveRange;    //사정거리
    public float reloadTime;        //장전시간
    public float perShot;           //한 발을 발사하는 데 걸리는 시간
}
