using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 毒蛛 : 技能基类
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        if (Random.Range(0, 2) == 1)
        {
            BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.减速);
        }
        if (Random.Range(0, 2) == 1)
        {
            BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.中毒);
        }
    }
}
