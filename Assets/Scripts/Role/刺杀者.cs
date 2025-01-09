using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 刺杀者 : 技能基类
{
    // Start is called before the first frame update

    public override void 战斗开始时()
    {
        ExecuteSkill();
    }

    public override void 技能帧()
    {
        角色实例.IsAtkChanged = true;

        foreach (var item in 角色实例.SkillPointIcon)
        {
            item.DOFade(1f, 0);
        }
    }
    public override void 攻击特效()
    {
        if (角色实例.IsAtkChanged)
        {
            BattleMgr.Ins.Targets[0].技能.TakeDamage(5);
            角色实例.IsAtkChanged = false;
            foreach (var item in 角色实例.SkillPointIcon)
            {
                item.DOFade(0.5f, 0);
            }
            角色实例.SkillPoint = 0;
        }
        else
        {
            //这里会设置IsSkillReady这个角色不需要
            角色实例.获取技能点();
            //永久保持false
            if (角色实例.SkillPoint == 角色实例.SkillPointMax)
            {
                角色实例.IsSkillReady = false;
                角色实例.IsAtkChanged = true;
            }
        }
    }

    public override void 获取攻击目标()
    {

        if(角色实例.IsAtkChanged)
        {
            BattleMgr.Ins.获取敌方阵营血量最低目标(角色实例.阵营);
        }
        else
        {
            base.获取攻击目标();
        }
    }
}
