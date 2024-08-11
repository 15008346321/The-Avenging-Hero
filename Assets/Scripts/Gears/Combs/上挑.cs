using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 上挑 : ComponentBaseComb
{
    public override void CombTargets()
    {
        BattleMgr.Ins.ShowSkillName(OwnerUnit, "上挑");
    }
}
