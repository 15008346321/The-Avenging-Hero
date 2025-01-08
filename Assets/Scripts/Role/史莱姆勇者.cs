using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 史莱姆勇者 : Unit
{
    public override void 战斗开始时()
    {
        ExecuteSkill();
    }

    public override void 回合开始时()
    {
        if (Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 20 && BattleMgr.Ins.AllUnit.Find(u=>u.name == "小水" && u.阵营 == 阵营) == null)
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
        小水 小水 = BattleMgr.Ins.InitRole(new UnitData(CSVMgr.Ins.Units["小水"], Cell), 阵营) as 小水;
        小水.召唤者 = this;
        小水.MaxHp += Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value * 2;
        if(Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 10)
        {
            小水.屏障已激活 = true;
        }
        if (Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 50)
        {

        }
        if (Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value > 100)
        {

        }
    }
}
