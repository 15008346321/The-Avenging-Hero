using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 风之袍 : ComponentBaseArmor
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Wind += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Wind -= 10;
        base.OnRemove();
    }
}
