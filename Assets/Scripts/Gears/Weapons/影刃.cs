using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 影刃 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Speed += 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Speed -= 5;
        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
    public override void OnAtk()
    {
        CurrSP += 1;
        if (CurrSP == SP)
        {
            OwnerUnit.NormalAtk.RemainAtkCount += 1;
            CurrSP = 0;
        }
    }
}
