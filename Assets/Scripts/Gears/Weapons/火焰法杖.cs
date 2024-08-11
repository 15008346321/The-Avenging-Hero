using System.Collections;
using UnityEngine;

public class 火焰法杖 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Fire += 10;
        OwnerUnitData.Water -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Fire -= 10;
        OwnerUnitData.Water += 5;

        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        base .OnRemove();
    }
}
