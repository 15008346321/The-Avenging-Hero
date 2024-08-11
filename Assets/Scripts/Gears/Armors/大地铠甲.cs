using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 大地铠甲 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Armor = this;
        OwnerUnitData.Earth += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Earth -= 10;
        OwnerUnitData.Armor = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
}
