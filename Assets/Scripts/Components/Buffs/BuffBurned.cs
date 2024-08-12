using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBurned : ComponentBaseBuff
{
    public void Init()
    {
        Name = "BuffBurned";
        MaxStack = 5;
    }

    public override void OnAdd(Unit from, Unit to)
    {
        base.OnAdd(from, to);
        BattleMgr.Ins.ShowFont(OwnerUnit, 0, "GetBurned");
        if (CurrStack < MaxStack)
        {
            CurrStack += 1;
        }
    }

    public override void OnTurnEnd()
    {
        //流血暂定掉血0.2攻击力
        BattleMgr.Ins.ShowFont(OwnerUnit, Mathf.RoundToInt(BuffFrom.Fire * 0.3f), "Burned");
        CurrStack -= 1;
        if (CurrStack <= 0)
        {
            OwnerUnit.Buffs.Remove(this);
        }
    }
}