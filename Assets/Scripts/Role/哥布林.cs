using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 哥布林 : Unit
{
    // Start is called before the first frame update

    public override void OnTurnStart()
    {
        int value;
        if (该单位是否是玩家阵营)
        {
            value = BattleMgr.Ins.玩家阵营单位列表.Count(item => !item.IsDead);
        }
        else
        {
            value = BattleMgr.Ins.敌人阵营单位列表.Count(item=>!item.IsDead);
        }
        Atk += value;

        StatePoolMgr.Ins.状态(this, "攻击力+" + value);
    }
}
