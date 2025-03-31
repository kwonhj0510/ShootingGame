using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float curHp;

    public Slider slider;

    private void Awake()
    {
        curHp = maxHp;
    }

    private void Update()
    {
        slider.value = curHp / maxHp;
    }
    public void TakeDamage(float damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
