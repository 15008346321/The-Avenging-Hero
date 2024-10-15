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
        if (!IsEnemy)
        {
            value = BattleMgr.Ins.Team.Count(item => !item.isDead);
        }
        else
        {
            value = BattleMgr.Ins.Enemys.Count(item=>!item.isDead);
        }
        Atk += value;

        StatePoolMgr.Ins.状态(this, "攻击力+" + value);
    }
}
