using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float curHp;

    public Slider slider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        curHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        slider.value = curHp / maxHp;
    }
    /// <summary>
    /// 피해를 입는 함수입니다.
    /// </summary>
    /// <param name="damage">입힐 피해량을 적어주세요.</param>
    public void TakeDamage(float damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
            //죽는 애니메이션
        }
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
