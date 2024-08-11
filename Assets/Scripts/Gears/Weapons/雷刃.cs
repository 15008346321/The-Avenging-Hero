using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 雷刃 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Thunder += 10;
        OwnerUnitData.Earth -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Thunder -= 10;
        OwnerUnitData.Earth += 5;
        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
}
