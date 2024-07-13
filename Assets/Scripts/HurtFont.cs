using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtFont : MonoBehaviour
{
    public int HurtValue;
    public int HealValue;
    //在伤害字体动画时事件调用

    public void PlayHurt()
    {
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = HurtValue.ToString();
        Animator animator = GetComponent<Animator>();
        animator.Play("hurt");
    }
    //动画事件 在hurt动画最后一帧调用
    public void UnitTakeDamage()
    {
        Unit u = transform.parent.parent.GetComponent<Unit>();
        u.Hp -= HurtValue;
        if (u.Hp < 0)
        {
            u.UnitDead();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHeal()
    {
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = HealValue.ToString();
        Animator animator = GetComponent<Animator>();
        animator.Play("hurt");
    }

    public void UnitHeal()
    {
        Unit u = transform.parent.parent.GetComponent<Unit>();
        u.Hp += HealValue;
        Destroy(gameObject);
    }
}
