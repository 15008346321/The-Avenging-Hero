using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBaseAtk: ComponentBase
{
    public int RemainAtkCount, TotalAtkCount;
    public void Init(string[] Data,Unit u)
    {
        base.Init(Data);
        TotalAtkCount =  int.Parse(Data[3]);
        OwnerUnit = u;
    }
    public virtual void GetTargets()
    {
        //获取目标
    }
    public virtual void AtkTargets()
    {
        //攻击目标
    }
}
