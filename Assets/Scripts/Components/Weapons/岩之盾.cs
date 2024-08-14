using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 岩之盾 : ComponentBaseWeapon
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Earth += 10;
        OwnerUnitData.Wind -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Earth -= 10;
        OwnerUnitData.Wind += 5;
        base .OnRemove();
    }
}
