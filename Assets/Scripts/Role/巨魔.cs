using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 巨魔 : Unit
{
    public override void 战斗开始时()
    {
        if(技能1已触发) return;
        ExecuteSkill();
        技能1已触发 = true;
    }

    public override void 技能帧()
    {
        召唤哥布林();
    }
    public override void 受到攻击时()
    {
        base.受到攻击时();
        if (生命值 * 2 < MaxHp && !技能2已触发)
        {
            技能2已触发 = true;
            IsSkillReady = true;
        }
    }

    public void 召唤哥布林()
    {
        BattleMgr.Ins.InitRole(new UnitData(CSVManager.Ins.Units["哥布林"], Cell), 阵营);
    }
}
