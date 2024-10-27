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
    public bool IsDebuff;
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
            Owner.BuffsList.Remove(this);
            if(Owner.BuffIcon.sprite.name == Name.ToString())
            {
                bool FindDebuff = false;
                foreach (var b in Owner.BuffsList)
                {
                    if (b.IsDebuff)
                    {
                        Owner.BuffIcon.sprite = CSVManager.Ins.TypeIcon[b.Name.ToString()];
                        FindDebuff = true;
                        break;
                    }
                }
                if (!FindDebuff)
                {
                    Owner.BuffIcon.enabled = false;
                }
            }
        }
    }

    public void DebuffOnAdd()
    {
        IsDebuff = true;
        Owner.BuffIcon.sprite = CSVManager.Ins.TypeIcon[Name.ToString()];
        Owner.BuffIcon.enabled = true;
    }
}

public enum BuffsEnum
{
    燃烧,
    盲目,
    出血,
}

public class 燃烧 : Buff
{
    public 燃烧(Unit u)
    {
        Name = BuffsEnum.燃烧;
        Owner = u;
        Stack = 1;
        Dscrp = "回合结束时受到最大生命值5%的伤害,燃烧存在时治疗无效";
        DebuffOnAdd();
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
        DebuffOnAdd();
    }
}

public class 出血 : Buff
{
    public 出血(Unit u)
    {
        Name = BuffsEnum.出血;
        Owner = u;
        Stack = 2;
        Dscrp = "回合结束时受到最大生命值5%的伤害,持续两回合";
        DebuffOnAdd();
    }
}
