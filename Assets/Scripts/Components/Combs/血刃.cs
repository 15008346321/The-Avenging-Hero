using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 血刃 : ComponentBaseComb
{
    public override void CombTargets()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "上挑");
    }
    public override void OnComb()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "血刃");
        foreach (var item in BattleMgr.Ins.Targets)
        {
            item.TakeAtkDamage(OwnerUnit, 0.3f);
        }
    }
}
