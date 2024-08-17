using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 气血铠甲 : ComponentBaseArmor
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.MaxHp += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.MaxHp -= 10;
        base.OnRemove();
    }
}
