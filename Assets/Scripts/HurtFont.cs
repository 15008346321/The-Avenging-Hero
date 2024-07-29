using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtFont : MonoBehaviour
{
    public int Value;
    public Unit u;
    public TextMeshProUGUI tmp;
    public Animator animator;
    
    //heal动画帧上调用
    public void UnitHeal()
    {
        u = transform.parent.parent.GetComponent<Unit>();
        u.Hp += Value;
        Destroy(gameObject);
    }
    //动画事件 在hurt动画第一帧调用
    public void UpdateHpBar()
    {
        u = transform.parent.parent.GetComponent<Unit>();
        u.Hp -= Value;
        u.UpdateHpBar();
        if (u.Hp <= 0)
        {
            u.UnitDead();
        }
    }
    //动画事件 在hurt动画最后一帧调用
    public void UnitCheckDeath()
    {
        u = transform.parent.parent.GetComponent<Unit>();
        if (u.isDead)
        {
            Destroy(u.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
