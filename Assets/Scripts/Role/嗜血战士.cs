using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 嗜血战士 : 技能基类
{
    // Start is called before the first frame update
    public bool 可以击退当前单个目标 = false;
    public override void 获取攻击目标()
    {
        BattleMgr.Ins.获取敌方前排单位(角色实例.阵营);
    }

    public override void 攻击帧()
    {
        base.攻击帧();
        BattleMgr.Ins.对目标群体加buff(BuffsEnum.出血);
    }

    public override void 回合结束时()
    {
        int count = BattleMgr.Ins.AllUnit.Where(u=>u.BuffsList.Exists(b=>b.Name == BuffsEnum.出血)).Count();
        if(count > 0)
        {
            角色实例.Atk += count;
            StatePoolMgr.Ins.状态(角色实例, "攻击力+" + count);

            float HealValue = 角色实例.MaxHp * 0.02f * count;
            TakeHeal(HealValue);
        }
    }
}
