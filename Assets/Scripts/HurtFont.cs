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
        transform.SetParent(parent.FontPos);
        transform.localPosition = Vector2.zero;
    }

    public void UnitHeal()
    {
        U.Hp += Value;
        Destroy(gameObject);
    }

    //动画事件 在hurt动画最后一帧调用
    public void UnitCheckDeath()
    {
        if (U.isDead)
        {
            U.transform.SetParent(BattleMgr.Ins.DeadParent);
            BattleMgr.Ins.CheckBattleEnd();
            if (BattleMgr.Ins.isBattling == false) return;
            BattleMgr.Ins.SortBySpeed();
        }
        Destroy(gameObject);
    }
}
