using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Buff
{
    public BuffsEnum Name;
    public string Dscrp;
    public Unit BuffFrom, Owner;
    public int Stack, MaxStack;
    public virtual void OnTurnEnd()
    {
        //实现自身逻辑 如减少层数
    }

    public void 层数减少(int num)
    {
        Stack -= num;

        Debug.Log("层数减少 curr:" + Stack);
        if (Stack <= 0)
        {
            Owner.Buffs.Remove(this);
        }
    }
}

public enum BuffsEnum
{
    燃烧,
    盲目,
}

public class 燃烧 : Buff
{
    public 燃烧(Unit u)
    {
        Name = BuffsEnum.燃烧;
        Owner = u;
        Stack = 1;
        Dscrp = "回合结束时造成最大生命值5%的伤害";
    }

    public override void OnTurnEnd()
    {
        float damage = Mathf.Round(Owner.MaxHp * 0.05f);
        Owner.TakeDamage(damage, DamageType.燃烧伤害);
        层数减少(1);
    }
}

public class 盲目 : Buff
{
    public 盲目(Unit u)
    {
        Name = BuffsEnum.盲目;
        Owner = u;
        Stack = 1;
        Dscrp = "无法攻击";
    }
}
