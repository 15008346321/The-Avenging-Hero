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
    public int CurrStack, MaxStack,OnAddStack = 1,BuffListIdx;
    public bool IsDebuff,IsStackable;
    public virtual void OnTurnEnd()
    {
        //实现自身逻辑 如减少层数
    }

    public void 层数改变(int num)
    {
        CurrStack += num;

        层数改变时特效(num);

        if (CurrStack <= 0)
        {
            Owner.BuffsList.Remove(this);
            for (int i = 0; i < Owner.BuffsList.Count; i++)
            {
                Owner.BuffListImgs[i].sprite = CSVMgr.Ins.TypeIcon[Owner.BuffsList[i].Name.ToString()];
            }
        }
    }

    public void DebuffOnAdd()
    {
        IsDebuff = true;

        for (int i = 0; i < Owner.BuffsList.Count; i++)
        {
            Owner.BuffListImgs[i].sprite = CSVMgr.Ins.TypeIcon[Owner.BuffsList[i].Name.ToString()];
        }
    }

    public virtual void 添加特效时() 
    { 
    }

    public virtual void 层数改变时特效(int value)
    {
    }
}

public class 燃烧 : Buff
{
    public 燃烧(Unit u)
    {
        Name = BuffsEnum.燃烧;
        Owner = u;
        CurrStack = OnAddStack;
        Dscrp = "回合结束时受到最大生命值5%的伤害,持续1回合,燃烧存在时治疗无效";
        DebuffOnAdd();
    }

    public override void OnTurnEnd()
    {
        float damage = Mathf.Round(Owner.MaxHp * 0.05f);
        Owner.TakeDamage(damage, ElementType.燃烧伤害,DamageType.异常伤害);
        层数改变(-1);
    }
}

public class 盲目 : Buff
{
    public 盲目(Unit u)
    {
        Name = BuffsEnum.盲目;
        Owner = u;
        CurrStack = OnAddStack;
        Dscrp = "无法攻击持续1回合";
        DebuffOnAdd();
    }

    public override void OnTurnEnd()
    {
        层数改变(-1);
    }
}

public class 出血 : Buff
{
    public 出血(Unit u)
    {
        Name = BuffsEnum.出血;
        Owner = u;
        OnAddStack = 2;
        CurrStack = OnAddStack;
        Dscrp = "回合结束时受到最大生命值5%的伤害,持续两回合";
        DebuffOnAdd();
    }

    public override void OnTurnEnd()
    {
        float damage = Mathf.Round(Owner.MaxHp * 0.05f);
        Owner.TakeDamage(damage, ElementType.出血伤害, DamageType.异常伤害);
        层数改变(-1);
    }
}

public class 麻痹 : Buff
{
    public 麻痹(Unit u)
    {
        Name = BuffsEnum.麻痹;
        Owner = u;
        CurrStack = OnAddStack;
        Dscrp = "无法获得技能点,持续1回合";
        DebuffOnAdd();
    }

    public override void OnTurnEnd()
    {
        层数改变(-1);
    }
}

public class 减速 : Buff
{
    public 减速(Unit u)
    {
        Name = BuffsEnum.减速;
        Owner = u;
        OnAddStack = 2;
        CurrStack = OnAddStack;
        Dscrp = "减少层数的速度,每回合减少一层";
        IsStackable = true;
        DebuffOnAdd();
        层数改变时特效(+2);
    }

    public override void 层数改变时特效(int value)
    {
        Owner.Speed -= value;
    }

    public override void OnTurnEnd()
    {
        层数改变(-1);
    }
}

public class 中毒 : Buff
{
    public 中毒(Unit u)
    {
        Name = BuffsEnum.中毒;
        Owner = u;
        OnAddStack = 2;
        CurrStack = OnAddStack;
        Dscrp = "回合结束时,受到层数*5%最大生命值的伤害";
        IsStackable = true;
        DebuffOnAdd();
    }

    public override void OnTurnEnd()
    {
        float damage = Mathf.Round(Owner.MaxHp * 0.05f * CurrStack);
        Owner.TakeDamage(damage, ElementType.中毒伤害, DamageType.异常伤害);
        层数改变(-1);
    }
}
