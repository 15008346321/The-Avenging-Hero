using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 猫妖 : 技能基类
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        if (Random.Range(0, 2) == 1)
        {
            BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.出血);
        }

        角色实例.获取技能点();
        //永久保持false
        if (角色实例.SkillPoint == 角色实例.SkillPointMax)
        {
            角色实例.AtkCountCurr += 1;
            StatePoolMgr.Ins.状态(角色实例, "攻击次数+1");
            角色实例.SkillPoint = 0;
            foreach (var item in 角色实例.SkillPointIcon)
            {
                item.DOFade(0.5f, 0);
            }
        }
    }
}
