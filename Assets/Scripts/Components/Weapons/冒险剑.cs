using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 冒险剑 : ComponentBase
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        OwnerUnitData.Atk += 10;
    }

    public override void OnRemove()
    {
        base .OnRemove();
        OwnerUnitData.Atk -= 10;
    }
}
