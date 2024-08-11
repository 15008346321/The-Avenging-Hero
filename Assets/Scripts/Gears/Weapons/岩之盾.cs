using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 岩之盾 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Earth += 10;
        OwnerUnitData.Wind -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Earth -= 10;
        OwnerUnitData.Wind += 5;
        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
}
