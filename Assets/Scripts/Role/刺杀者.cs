using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 刺杀者 : Unit
{
    // Start is called before the first frame update

    public override void 战斗开始时()
    {
        ExecuteSkill();
    }

    public override void 技能帧()
    {
        IsAtkChanged = true;

        print("cishazhe OnBattleStart");
        foreach (var item in SkillPointIcon)
        {
            item.DOFade(1f, 0);
        }
    }
    public override void 攻击特效()
    {
        if (IsAtkChanged)
        {
            BattleMgr.Ins.Targets[0].TakeDamage(5);
            IsAtkChanged = false;
            foreach (var item in SkillPointIcon)
            {
                item.DOFade(0.5f, 0);
            }
            SkillPoint = 0;
        }
        else
        {
            //这里会设置IsSkillReady这个角色不需要
            获取技能点();
            //永久保持false
            if (SkillPoint == SkillPointMax)
            {
                IsSkillReady = false;
                IsAtkChanged = true;
            }
        }
    }

    public override void 获取攻击目标()
    {

        if(IsAtkChanged)
        {
            BattleMgr.Ins.获取敌方阵营血量最低目标(阵营);
        }
        else
        {
            base.获取攻击目标();
        }
    }
}
