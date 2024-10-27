using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手 : Unit
{
    public override void 攻击特效()
    {
        SkillPoint++;
        SkillPointIcon[SkillPoint - 1].DOFade(1, 0);
        if (SkillPoint == SkillPointMax)
        {
            IsSkillReady = true;
        }
    }

    public override void 获取技能目标()
    {
        BattleMgr.Ins.获取最多单位一行的目标(Cell, IsEnemy);
    }

    public override void 技能帧()
    {
        BattleMgr.Ins.CaculDamage(Atk);
    }
}
