using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtFont : MonoBehaviour
{
    public int Value;
    public int HealValue;
    public Unit u;
    public TextMeshProUGUI tmp;
    //在伤害字体动画时事件调用

    public void PlayHurt()
    {
        print("msg 2");
        tmp.text = Value.ToString();
        Animator animator = GetComponent<Animator>();
        animator.Play("hurt");
    }
    public void PlayHeal()
    {
        tmp.text = HealValue.ToString();
        Animator animator = GetComponent<Animator>();
        animator.Play("heal");
    }
    public void PlayBleed()
    {
        tmp.text = Value.ToString();
        Animator animator = GetComponent<Animator>();
        animator.Play("Bleed");
    }
    //heal动画帧上调用
    public void UnitHeal()
    {
        Unit u = transform.parent.parent.GetComponent<Unit>();
        u.Hp += HealValue;
        Destroy(gameObject);
    }
    //动画事件 在hurt动画第一帧调用
    public void UpdateHpBar()
    {
        u = transform.parent.parent.GetComponent<Unit>();
        u.Hp -= Value;
        u.UpdateHpBar();
        if (u.Hp < 0)
        {
            u.UnitDead();
        }
    }
    //动画事件 在hurt动画最后一帧调用
    public void UnitCheckDeath()
    {
        if (u.isDead)
        {
            Destroy(u.transform.parent.gameObject);
            BattleMgr.Ins.CheckBattleEnd();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
