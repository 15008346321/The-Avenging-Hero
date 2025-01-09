using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 格斗家 : 技能基类
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.减速);
        var damage = 角色实例.Speed - BattleMgr.Ins.Targets[0].Speed;
        if(damage > 0)
        {
            BattleMgr.Ins.Targets[0].技能.TakeDamage(damage, ElementType.土元素伤害);
        }
    }
}
