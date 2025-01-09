using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 法师 : 技能基类
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        float damage = (float)角色实例.Bloods.Find(item => item.Name == 魔力类型Enum.火元素)?.Value * 0.5f;

        BattleMgr.Ins.Targets[0].技能.TakeDamage(damage,ElementType.火元素伤害);
        BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.燃烧);
    }
}
