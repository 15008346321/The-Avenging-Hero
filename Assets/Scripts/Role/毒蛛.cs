using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 毒蛛 : Unit
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        if (Random.Range(0, 2) == 1)
        {
            BattleMgr.Ins.Targets[0].AddBuff(BuffsEnum.减速);
        }
        if (Random.Range(0, 2) == 1)
        {
            BattleMgr.Ins.Targets[0].AddBuff(BuffsEnum.中毒);
        }
    }
}
