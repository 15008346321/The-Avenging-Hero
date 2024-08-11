using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 狂风手套 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Wind += 10;
        OwnerUnitData.Fire -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Wind -= 10;
        OwnerUnitData.Fire += 5;
        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
}
