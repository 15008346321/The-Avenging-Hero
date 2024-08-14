using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 雷刃 : ComponentBaseWeapon
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Thunder += 10;
        OwnerUnitData.Earth -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Thunder -= 10;
        OwnerUnitData.Earth += 5;
        base .OnRemove();
    }
}
