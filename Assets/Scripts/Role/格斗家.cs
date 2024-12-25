using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 格斗家 : Unit
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.减速);
        var damage = Speed - BattleMgr.Ins.Targets[0].Speed;
        if(damage > 0)
        {
            BattleMgr.Ins.Targets[0].TakeDamage(damage, ElementType.土元素伤害);
        }
    }
}
