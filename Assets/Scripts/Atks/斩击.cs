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
        BattleMgr.Ins.Targets.Clear();
        Unit u = OwnerUnit.GetOppositeTarget();
        if (u != null)
        {
            BattleMgr.Ins.Targets.Add(u);
            BattleMgr.Ins.MainTarget = BattleMgr.Ins.Targets[0];
        }
    }

    public override void AtkTargets()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "斩击");
        foreach (var item in BattleMgr.Ins.Targets)
        {
            item.TakeAtkDamage(OwnerUnit,1);
        }
    }

    public override void AddEffectAfterAtk()
    //在这里加异常特性buff等
    {
        OwnerUnit.Atk++;
        BattleMgr.Ins.ShowFont(OwnerUnit, "攻击力+1");
        if (BattleMgr.Ins.Targets.Count <= 0) return;
        if (!BattleMgr.Ins.TrySuccess(0.3f)) return;
        foreach (var item in BattleMgr.Ins.Targets)
        {
            BuffBleed existingBuff = item.Buffs.OfType<BuffBleed>().FirstOrDefault();
            if (existingBuff == null)
            {
                BuffBleed newBuff = new();
                item.Buffs.Add(newBuff);
                newBuff.Init(OwnerUnit, item);
            }
            else
            {
                existingBuff.AddLayer();
            }
        }
        OwnerUnit.CheckComb("流血");
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
