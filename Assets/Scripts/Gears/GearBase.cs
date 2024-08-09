using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GearBase
{
    public int ID, Level;
    public string Type, Name, Dscrp;
    public int CurrXP, SP, CurrSP;
    public int[] XP = {5,15,30,50,80};
    public UnitData OwnerUnitData;
    public Unit OwnerUnit;
    public Sprite sprite;
    public Transform StartParent;
    public void Init(string[] Data,int Lv=0)
    {
        Level = Lv;
        ID = int.Parse(Data[0]);
        Name = Data[1];
        Dscrp = Data[2];
        Type = Data[3];

        Debug.Log(Name + Type);
    }
    public abstract void OnAdd(UnitData owner);
    public abstract void OnRemove();
    public abstract void Upgrade();
    public abstract void OnAtk();
    public abstract void OnComb();
    public abstract void OnFire();
    public abstract void OnWater();
    public abstract void OnWind();
    public abstract void OnThunder();
    public abstract void OnEarth();
    public abstract void OnHeal();
    public abstract void OnShield();
    public abstract void Test();
}
