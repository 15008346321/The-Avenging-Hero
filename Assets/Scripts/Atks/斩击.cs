using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class 斩击 : AtkBase
{
    public override void OnAdd()
    {
        throw new System.NotImplementedException();
    }

    public override void GetTargets()
    {
        Targets.Add(OwnerUnit.GetOppositeTarget());
    }

    public override void AtkTargets()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "斩击");
        foreach (var item in Targets)
        {
            item.TakeAtkDamage(OwnerUnit,1);
        }
    }

    public override void AddEffectAfterAtk()
    //在这里加异常特性buff等
    {
        OwnerUnit.Atk++;
        BattleMgr.Ins.ShowFont(OwnerUnit, "攻击力+1");
        if (Targets.Count <= 0) return;
        if (!BattleMgr.Ins.TrySuccess(0.3f)) return;
        BuffBleed existingBuff = BattleMgr.Ins.AtkedU.Buffs.OfType<BuffBleed>().FirstOrDefault();
        if (existingBuff == null)
        {
            BuffBleed newBuff = new();
            BattleMgr.Ins.AtkedU.Buffs.Add(newBuff);
            newBuff.Init(OwnerUnit, BattleMgr.Ins.AtkedU);
        }
        else
        {
            existingBuff.AddLayer();
        }
        OwnerUnit.CheckComb("BuffBleed");
    }

    public override void OnRemove()
    {
        throw new System.NotImplementedException();
    }

    public override void Test()
    {
        Debug.Log("这是斩击");
    }
}
