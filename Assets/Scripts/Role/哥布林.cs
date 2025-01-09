using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 哥布林 : 技能基类
{
    // Start is called before the first frame update

    public override void 回合开始时()
    {
        int value;
        if (角色实例.阵营 == 阵营Enum.我方)
        {
            value = BattleMgr.Ins.玩家阵营单位列表.Count(item => !item.IsDead);
        }
        else
        {
            value = BattleMgr.Ins.敌人阵营单位列表.Count(item=>!item.IsDead);
        }
        角色实例.Atk += value;

        StatePoolMgr.Ins.状态(角色实例, "攻击力+" + value);
    }
}
