using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 商人 : Unit
{
    // Start is called before the first frame update
    public override void 战斗结束时()
    {
        EventsMgr.Ins.玩家拥有的金币 += 1;
        EventsMgr.Ins.获取空侧边栏并展示("金币+1");
        int value;
        if (EventsMgr.Ins.玩家拥有的金币 > 10)
        {
            value = EventsMgr.Ins.玩家拥有的金币 / 10;
            EventsMgr.Ins.玩家拥有的金币 += value;
            EventsMgr.Ins.获取空侧边栏并展示("金币+"+value);
        }
    }
}
