using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 血刃 : CombBase
{
    // Start is called before the first frame update

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
