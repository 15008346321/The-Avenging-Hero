using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AtkBase
{
    public int ID;
    public string Name;
    public string Dscrp;
    public int TotalAtkCount;
    public int RemainAtkCount;
    public Unit OwnerUnit;
    public void Init(Unit Owner, string[] Data) 
    {
        OwnerUnit = Owner;
        ID = int.Parse(Data[0]);
        Name = Data[1];
        Dscrp = Data[2];
        TotalAtkCount = int.Parse(Data[3]);
    }
    public abstract void OnAdd();
    public abstract void GetTargets();
    public abstract void AtkTargets();
    public abstract void AddEffectAfterAtk();
    public abstract void OnRemove();
    public abstract void Test();
}
