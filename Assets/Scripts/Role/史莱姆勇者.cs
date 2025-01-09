using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 史莱姆勇者 : 技能基类
{
    public override void 战斗开始时()
    {
        ExecuteSkill();
    }

    public override void 回合开始时()
    {
        if (角色实例.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 20 && BattleMgr.Ins.AllUnit.Find(u=>u.name == "小水" && u.阵营 == 角色实例.阵营) == null)
        {
            ExecuteSkill();
        }
    }

    public override void 技能帧()
    {
        召唤小水();
    }

    public void 召唤小水()
    {
        Unit 小水 = BattleMgr.Ins.InitRole(new UnitData(CSVMgr.Ins.Units["小水"], 设置召唤物位置()), 角色实例.阵营);
        if (小水.技能 is 小水 小水技能)
        {
            小水技能.召唤者 = 角色实例;
            小水.MaxHp += 角色实例.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value * 2;
            if (角色实例.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 10)
            {
                小水技能.屏障已激活 = true;
            }
            if (角色实例.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 50)
            {

            }
            if (角色实例.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 100)
            {

            }
        }
    }
}
