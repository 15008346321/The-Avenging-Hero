using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBurned : BuffBase
{
    public override void Init(Unit from, Unit target)
    {
        ID = 1;
        MaxStack = 1;
        CurrStack = 0;
        BuffFrom = from;
        BuffTarget = target;
        AddLayer();
    }

    public override void AddLayer()
    {
        BattleMgr.Ins.ShowFont(BuffTarget, 0, "GetBurned");
        if (CurrStack < MaxStack)
        {
            CurrStack += 1;
        }
    }
    public override void OnAttack()
    {
    }

    public override void OnAttacked()
    {
    }

    public override void OnComb()
    {
    }

    public override void OnCombed()
    {
    }

    public override void OnTurnEnd()
    {
        //流血暂定掉血0.2攻击力
        BattleMgr.Ins.ShowFont(BuffTarget, Mathf.RoundToInt(BuffFrom.Fire * 0.3f), "Burned");
        CurrStack -= 1;
        if (CurrStack <= 0)
        {
            BuffTarget.Buffs.Remove(this);
        }
    }

    public override void OnTurnStart()
    {
    }

    public override void Remove()
    {
    }
}
