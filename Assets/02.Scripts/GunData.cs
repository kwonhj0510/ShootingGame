using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class GunData : ScriptableObject
{
    public string name;       //�̸�
    public float damage;              //������
    public int magazine;            //źâ ��
    public float effectiveRange;    //�����Ÿ�
    public float reloadTime;        //�����ð�
    public float perShot;           //�� ���� �߻��ϴ� �� �ɸ��� �ð�
}
