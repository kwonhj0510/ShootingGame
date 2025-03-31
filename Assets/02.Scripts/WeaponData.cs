using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;       //�̸�
    public float damage;              //������
    public int magazine;            //źâ ��
    public float effectiveRange;    //�����Ÿ�
    public float reloadTime;        //�����ð�
    public float perShot;           //�� ���� �߻��ϴ� �� �ɸ��� �ð�
}
