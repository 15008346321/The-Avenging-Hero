using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手 : Unit
{
    public override void 攻击特效()
    {
        获取技能点() ;
    }

    public override void 获取技能目标()
    {
        BattleMgr.Ins.获取敌方单位最多的一行的所有目标(Cell, 阵营);
    }

    public override void 技能帧()
    {
        BattleMgr.Ins.CaculDamage(Atk);
    }
}
