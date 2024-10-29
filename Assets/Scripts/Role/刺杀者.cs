using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 刺杀者 : Unit
{
    // Start is called before the first frame update

    public override void OnBattleStart()
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
            获取技能点();
        }
    }

    public override void 获取攻击目标()
    {

        if(IsAtkChanged)
        {
            BattleMgr.Ins.获取阵营血量最低目标(!IsEnemy);
        }
        else
        {
            base.获取攻击目标();
        }
    }
}
