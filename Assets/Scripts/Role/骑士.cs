using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骑士 : Unit
{
    public override void 战斗开始时()
    {
        ExecuteSkill();
        //避免这个阶段触发2技能
        技能2已触发 = true;
    }

    public override void 技能帧()
    {
        if (!技能1已触发) 
        {
            foreach (var item in BattleMgr.Ins.玩家阵营单位列表)
            {
                item.获取护盾(Mathf.Round(MaxHp * 0.2f));
            }
            技能1已触发 = true;
        }

        if (!技能2已触发)
        {
            获取护盾(Mathf.Round(MaxHp * 0.05f));
        }

    }

    public override void OnTurnStart()
    {
        技能2已触发 = false;
        ExecuteSkill();
    }
}
