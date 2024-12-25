using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 火焰剑士 : Unit
{
    public override void 攻击特效()
    {
        float damage = (float)Bloods.Find(item => item.Name == 魔力类型Enum.火元素)?.Value *0.3f;
        BattleMgr.Ins.Targets[0].TakeDamage(damage, ElementType.火元素伤害);

        if (BattleMgr.Ins.Targets[0].BuffsList.Exists(b=>b.Name==BuffsEnum.燃烧))
        {
            BattleMgr.Ins.Targets[0].BuffsList.Find(b => b.Name == BuffsEnum.燃烧).OnTurnEnd();
            获取技能点();
        }
    }

    public override void 技能帧()
    {
        AtkCountCurr += 1;
        永久添加血脉(魔力类型Enum.火元素, 1);
        临时添加血脉(魔力类型Enum.火元素, 1);
    }
}
