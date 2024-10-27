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
            SkillPoint++;
            SkillPointIcon[SkillPoint - 1].DOFade(1, 0);
            if (SkillPoint == SkillPointMax)
            {
                IsAtkChanged = true;
            }
        }
    }

    public override void 获取攻击目标()
    {

        print("csz获取攻击目标");

        print("IsAtkChanged: " + IsAtkChanged.ToString());
        if(IsAtkChanged)
        {
            print("1");
            BattleMgr.Ins.获取阵营血量最低目标(!IsEnemy);
        }
        else
        {

            print("2");
            base.获取攻击目标();
        }
    }
}
