using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 燃烧 : BuffBase
{
    public 燃烧(Unit buffto)
    {
        BuffTo = buffto;
        stack = 1;
        Dscrp = "回合结束时造成最大生命值5%的伤害";
    }

    public override void OnTurnEnd()
    {
        float damage = Mathf.Round( BuffTo.MaxHp * 0.05f);
        BuffTo.TakeDamage(damage, DamageType.燃烧伤害);
        base.OnTurnEnd();
    }
}
