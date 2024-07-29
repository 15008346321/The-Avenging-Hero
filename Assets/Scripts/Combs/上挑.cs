using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 上挑 : CombBase
{
    // Start is called before the first frame update
    public 上挑(Unit from, List<Unit> atkTo) : base(from, atkTo)
    {
        TotalCombCount = 1;
        RemainCombCount = 1;
        CombTypes.Add("BuffBleed");
    }

    public override void AddEffectAfterComb()
    {
        throw new System.NotImplementedException();
    }

    public override void CombTargets()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "血刃");
    }

    public override void GetTargets()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAdd()
    {
        throw new System.NotImplementedException();
    }

    public override void OnRemove()
    {
        throw new System.NotImplementedException();
    }
}
