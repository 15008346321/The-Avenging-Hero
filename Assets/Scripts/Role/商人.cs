using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 商人 : Unit
{
    // Start is called before the first frame update
    public override void 战斗结束时()
    {
        StartCoroutine(BagMgr.Ins.金币变动(1));
        int value;
        if (BagMgr.Ins.玩家拥有的金币 > 10)
        {
            value = BagMgr.Ins.玩家拥有的金币 / 10;
            BagMgr.Ins.玩家拥有的金币 += value;
            StartCoroutine(BagMgr.Ins.金币变动(value));
        }
    }
}
