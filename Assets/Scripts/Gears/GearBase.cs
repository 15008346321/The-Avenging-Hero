using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GearBase : MonoBehaviour
{
    public int ID, Level;
    public string Type, Name, Dscrp;
    public int TotalAtkCount, RemainAtkCount, CurrXP, SP;
    public int[] XP = {5,15,30,50,80};
    public Unit OwnerUnit;
    public Sprite sprite;
    public void Init(Unit Owner, string[] Data,int Lv)
    {
        OwnerUnit = Owner;
        Level = Lv;
        ID = int.Parse(Data[0]);
        Name = Data[1];
        Dscrp = Data[2];
        TotalAtkCount = int.Parse(Data[3]);
        OnAdd();
    }
    public abstract void OnAdd();
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
