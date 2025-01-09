using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 史莱姆 : 技能基类
{
    // Start is called before the first frame update
    public override void 受到攻击时()
    {
        base.受到攻击时();
        if (角色实例.生命值 * 2 < 角色实例.MaxHp && !技能1已触发)
        {
            分裂();
        }
    }

    public void 分裂()
    {
        角色实例.IsDead = true;
        技能1已触发 = true;
        角色实例.CheckDeath();
        StatePoolMgr.Ins.状态(角色实例, "分裂");

        UnitData data = new(CSVMgr.Ins.Units["史莱姆"], 角色实例.Cell);

        float 阵营scale = 角色实例.阵营 == 阵营Enum.我方 ? 1 : -1;

        Unit u1 = BattleMgr.Ins.InitRole(data, 角色实例.阵营);
        u1.技能.技能1已触发 = true;
        u1.transform.localScale = 0.8f * new Vector2(1 * 阵营scale, 1);

        Unit u2 = BattleMgr.Ins.InitRole(data, 角色实例.阵营);
        u2.技能.技能1已触发 = true;
        u2.transform.localScale = 0.8f * new Vector2(1 * 阵营scale, 1);
    }
}
