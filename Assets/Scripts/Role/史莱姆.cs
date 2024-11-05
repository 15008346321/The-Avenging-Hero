using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 史莱姆 : Unit
{
    // Start is called before the first frame update
    public override void 受到攻击时()
    {
        base.受到攻击时();
        if (Hp * 2 < MaxHp && !技能1已触发)
        {
            分裂();
        }
    }

    public void 分裂()
    {
        IsDead = true;
        技能1已触发 = true;
        transform.SetParent(BattleMgr.Ins.DeadParent);
        StatePoolMgr.Ins.状态(this, "分裂");

        UnitData data = new UnitData(CSVManager.Ins.Units["史莱姆"], Cell);

        Unit u1 = BattleMgr.Ins.InitRole(data, 该单位是否是玩家阵营);
        u1.技能1已触发 = true;
        u1.transform.localScale = Vector3.one*0.8f;

        Unit u2 = BattleMgr.Ins.InitRole(data, 该单位是否是玩家阵营);
        u2.技能1已触发 = true;
        u2.transform.localScale = Vector3.one * 0.8f;
    }
}
