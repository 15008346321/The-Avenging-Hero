using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 赤炎铠甲 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData = owner;
        OwnerUnitData.Armor = this;
        OwnerUnitData.Fire += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Fire -= 10;
        OwnerUnitData.Armor = null;
        OwnerUnitData = null;
        base .OnRemove();
    }

    public override void Test()
    {
        Debug.Log("装备了赤血剑");
    }
}
