using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 猫妖 : Unit
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        if (Random.Range(0, 2) == 1)
        {
            BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.出血);
        }

        获取技能点();
        //永久保持false
        if (SkillPoint == SkillPointMax)
        {
            AtkCountCurr += 1;
            StatePoolMgr.Ins.状态(this,"攻击次数+1");
            SkillPoint = 0;
            foreach (var item in SkillPointIcon)
            {
                item.DOFade(0.5f, 0);
            }
        }
    }
}
