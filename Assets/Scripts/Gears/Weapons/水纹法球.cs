using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 水纹法球 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Water += 10;
        OwnerUnitData.Thunder -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {

        OwnerUnitData.Water -= 10;
        OwnerUnitData.Thunder += 5;
        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
}
