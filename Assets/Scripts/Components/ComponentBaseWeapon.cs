using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBaseWeapon: ComponentBase
{
    public override void Init(string[] Data)
    {
        base.Init(Data);
        Type = "武器";
    }

    public override void OnAdd(UnitData owner)
    {
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
        TeamManager.Ins.ShowTeam();
    }
}
