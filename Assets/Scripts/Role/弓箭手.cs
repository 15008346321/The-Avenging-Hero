using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手 : 技能基类
{
    public override void 攻击特效()
    {
        角色实例.获取技能点() ;
    }

    public override void 获取技能目标()
    {
        BattleMgr.Ins.获取敌方单位最多的一行的所有目标(角色实例.Cell, 角色实例.阵营);
    }

    public override void 技能帧()
    {
        BattleMgr.Ins.CaculDamage(角色实例.Atk);
    }
}
