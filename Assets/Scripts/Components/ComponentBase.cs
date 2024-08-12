using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBase
{
    public int ID, Level;
    public string Type, Name, Dscrp;
    public int CurrXP, SP, CurrSP;
    public int[] XP = {5,15,30,50,80};
    public UnitData OwnerUnitData;
    public Unit OwnerUnit;
    public Sprite sprite;
    public Transform StartParent;

    public virtual void Init(string[] Data)
    {
        ID = int.Parse(Data[0]);
        Name = Data[1];
        Dscrp = Data[2];
    }
    public virtual void OnAdd(UnitData owner)
    {

    }
    public virtual void OnRemove() 
    {
        
    }
    public virtual void Upgrade()
    {
        //装备升级 暂时不做
    }
    public virtual void OnAtk()
    {
        //造成普通攻击时
    }
    public virtual void OnComb()
    {
        //造成追打时
    }
    public virtual void OnPhs()
    {
        //造成物伤
    }
    public virtual void OnFire()
    {
        //造成火伤
    }
    public virtual void OnWater()
    {
        //造成水伤
    }
    public virtual void OnWind()
    {
        //造成风伤
    }
    public virtual void OnThunder()
    {
        //造成雷伤
    }
    public virtual void OnEarth()
    {
        //造成土伤
    }
    public virtual void OnHeal()
    {
        //提供治疗
    }
    public virtual void OnShield()
    {
        //提供护盾
    }
    public virtual void OnTurnStart()
    {
        //回合开始
    }
    public virtual void OnTurnEnd()
    {
        //回合结束
    }
    public virtual void OnBattleEnd()
    {
        //战斗结束
    }
    public virtual void RcAtk()
    {
        //受到普攻
    }
    public virtual void RcComb()
    {
        //受到追打
    }
    public virtual void RcPhs()
    {
        //受到物伤
    }
    public virtual void RcFire()
    {
        //受到火伤
    }
    public virtual void RcWater()
    {
        //受到水伤
    }
    public virtual void RcWind()
    {
        //受到风伤
    }
    public virtual void RcThunder()
    {
        //受到雷伤
    }
    public virtual void RcEarth()
    {
        //受到土伤
    }
    public virtual void RcHeal()
    {
        //受到治疗
    }
    public virtual void RcShield()
    {
        //受到护盾
    }
    public virtual void Test()
    {

    }
}
