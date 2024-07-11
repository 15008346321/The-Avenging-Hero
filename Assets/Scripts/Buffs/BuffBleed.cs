using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBleed : Buff
{
    public override void Init(Unit from, Unit target)
    {
        ID = 1;
        MaxStack = 5;
        CurrStack = 1;
        BuffFrom = from;
        BuffTarget = target;
    }

    public override void AddLayer()
    {
        if (CurrStack < MaxStack)
        {
            CurrStack += 1;
        }
    }
    public override void OnAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAttacked()
    {
        throw new System.NotImplementedException();
    }

    public override void OnComb()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCombed()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTurnEnd()
    {
        //流血暂定掉血0.2攻击力
        BuffTarget.TakeDamage(Mathf.RoundToInt(BuffFrom.Atk*0.2f)*CurrStack);
    }

    public override void OnTurnStart()
    {
        throw new System.NotImplementedException();
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
