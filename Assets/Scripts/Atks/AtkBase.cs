using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AtkBase
{
    public string Name;
    public int ID;
    public int TotalAtkCount;
    public int RemainAtkCount;
    public string Dscrp;
    public Unit OwnerUnit;
    public List<Unit> Targets;
    public void Init(Unit from, string[] Data) 
    {
        OwnerUnit = from;
    }
    public abstract void OnAdd();
    public abstract void GetTargets();
    public abstract void AtkTargets();
    public abstract void AddEffectAfterAtk();
    public abstract void OnRemove();
    public abstract void Test();
}
