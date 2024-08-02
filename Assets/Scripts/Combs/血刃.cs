using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 血刃 : CombBase
{
    public override void OnAdd()
    {
        throw new System.NotImplementedException();
    }
    public override void GetTargets()
    {
        
    }

    public override void CombTargets()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "血刃");
        foreach (var item in BattleMgr.Ins.Targets)
        {
            item.TakeAtkDamage(OwnerUnit, 0.3f);
        }
    }

    public override void AddEffectAfterComb()
    {
    }

    public override void OnRemove()
    {
        throw new System.NotImplementedException();
    }
}
