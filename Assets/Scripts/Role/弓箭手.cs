using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手 : Unit
{
    // Start is called before the first frame update
    List<int> BehindCells = new() { 7,8,9 };
    public bool repelTarget = false;

    public override void 攻击特效()
    {
        SkillPoint++;
        SkillPointIcon[SkillPoint - 1].DOFade(1, 0);
        if (SkillPoint == SkillPointMax)
        {
            isSkillReady = true;
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
