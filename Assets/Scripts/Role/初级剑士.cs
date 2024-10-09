using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 剑士 : Unit
{
    // Start is called before the first frame update
    
    public override void GetTargets()
    {
        base.GetTargets();
        if(BattleMgr.Ins.Targets.Count > 0)
        {
            Unit t2 = BattleMgr.Ins.FindUnitOnCell(BattleMgr.Ins.Targets[0].Cell + 3, IsEnemy);
            if (t2 != null)
            {
                BattleMgr.Ins.Targets.Add(t2);
            }
        }
    }
}
