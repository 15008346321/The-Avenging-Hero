using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 冒险剑 : ComponentBaseWeapon
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Atk += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Atk -= 10;
        base.OnRemove();
    }
}
