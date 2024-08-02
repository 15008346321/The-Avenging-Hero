using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtFont : MonoBehaviour
{
    public int Value;
    public Unit U;
    public TextMeshProUGUI tmp;
    public Animator animator;
    
    //heal动画帧上调用

    public void Init(Unit parent,string text,int value = 0)
    {
        U = parent;
        tmp.text = text;
        Value = value;
    }

    public void UnitHeal()
    {
        U.Hp += Value;
        Destroy(gameObject);
    }
    //动画事件 在hurt动画第一帧调用
    public void UpdateHpBar()
    {
        U.Hp -= Value;
        U.UpdateHpBar();
        if (U.Hp <= 0)
        {
            U.isDead = true;
        }
    }
    //动画事件 在hurt动画最后一帧调用
    public void UnitCheckDeath()
    {
        if (U.isDead)
        {
            U.transform.parent.SetParent(BattleMgr.Ins.DeadParent);
            if (BattleMgr.Ins.CheckBattleEnd())
            {
                BattleMgr.Ins.ExitBattle();
            }
        }
        Destroy(gameObject);
    }

}
