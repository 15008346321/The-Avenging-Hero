using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBleed : ComponentBaseBuff
{
    public void Init()
    {
        Name = "BuffBleed";
        MaxStack = 5;
    }

    public override void OnAdd(Unit from = null, Unit to = null)
    {
        base.OnAdd(from,to);
        BattleMgr.Ins.ShowFont(OwnerUnit, 0, "GetBleed");
        if (CurrStack < MaxStack)
        {
            CurrStack += 1;
        }
    }

    public override void OnTurnEnd()
    {
        //流血暂定掉血0.2攻击力
        BattleMgr.Ins.ShowFont(OwnerUnit, Mathf.RoundToInt(BuffFrom.Atk * 0.2f) * CurrStack, "Bleed");
        CurrStack -= 1;
        if (CurrStack <= 0)
        {
            OwnerUnit.Buffs.Remove(this);
        }
    }
}
