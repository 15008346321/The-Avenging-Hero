using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 巨魔 : 技能基类
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
        if (角色实例.生命值 * 2 < 角色实例.MaxHp && !技能2已触发)
        {
            技能2已触发 = true;
            角色实例.IsSkillReady = true;
        }
    }

    public void 召唤哥布林()
    {
        BattleMgr.Ins.实例化角色(new UnitData(CSVMgr.Ins.Units["哥布林"], 角色实例.Cell), 角色实例.阵营);
    }
}
