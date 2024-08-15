using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 史莱姆屏障 : ComponentBaseSupport
{
    public override void OnAdd(UnitData owner)
    {
        base.OnAdd(owner);
        SP = 1;
    }

    public override void RcAtk()
    {
        if (SP > 0)
        {
            OwnerUnit.Damage = 0;
        }
    }
}
