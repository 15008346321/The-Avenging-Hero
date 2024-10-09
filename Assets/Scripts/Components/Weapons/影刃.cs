using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 影刃 : ComponentBaseWeapon
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Speed += 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Speed -= 5;
        base .OnRemove();
    }
    public override void OnAtk()
    {
        CurrSP += 1;
        if (CurrSP == SP)
        {
            OwnerUnit.AtkCountCurr += 1;
            CurrSP = 0;
        }
    }
}
