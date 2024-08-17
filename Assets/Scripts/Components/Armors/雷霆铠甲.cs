using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 雷霆铠甲 : ComponentBaseArmor
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Thunder += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Thunder -= 10;
        base.OnRemove();
    }
}
