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
        if (Hp * 2 < MaxHp)
        {
            分裂();
        }
    }

    public void 分裂()
    {
        if (isSkillTriggered) return;

        Unit u1 = BattleMgr.Ins.SummonCreator(gameObject, IsEnemy, 0.8f);
        u1.isSkillTriggered = true;
        Unit u2 = BattleMgr.Ins.SummonCreator(gameObject, IsEnemy, 0.8f);
        u2.isSkillTriggered = true;

        isDead = true;
        transform.SetParent(BattleMgr.Ins.DeadParent);
        StatePoolMgr.Ins.状态(this, "分裂+");
    }
}
