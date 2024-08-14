using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBaseArmor: ComponentBase
{
    public override void Init(string[] Data)
    {
        base.Init(Data);
        Type = "防具";
    }
    public override void OnAdd(UnitData owner)
    {
        OwnerUnitData = owner;
        OwnerUnitData.Armor = this;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Armor = null;
        OwnerUnitData = null;
        TeamManager.Ins.ShowTeam();
    }
}
