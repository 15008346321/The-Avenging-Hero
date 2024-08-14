using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBaseSupport: ComponentBase
{
    public override void Init(string[] Data)
    {
        base.Init(Data);
        Type = "辅助";
    }

    public override void OnAdd(UnitData owner)
    {
        OwnerUnitData = owner;
        OwnerUnitData.Support = this;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Support = null;
        OwnerUnitData = null;
        TeamManager.Ins.ShowTeam();
    }
}
