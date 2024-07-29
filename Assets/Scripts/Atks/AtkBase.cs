using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AtkBase
{
    public int ID;
    public int TotalAtkCount;
    public int RemainAtkCount;
    public string Name;
    public string Dscrp;
    public Unit OwnerUnit;
    public List<Unit> Targets;
    public AtkBase(Unit from) 
    {
        OwnerUnit = from;
    }
    public abstract void OnAdd();
    public abstract void GetTargets();
    public abstract void AtkTargets();
    public abstract void AddEffectAfterAtk();
    public abstract void OnRemove();
    public abstract void test();
}
