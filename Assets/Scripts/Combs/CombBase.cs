using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CombBase
{
    public int ID;
    public int TotalCombCount;
    public int RemainCombCount;
    public string Name;
    public string Dscrp;
    public Unit OwnerUnit;
    public List<Unit> Targets;
    public List<string> CombTypes;

    public CombBase(Unit from, List<Unit> atkTo) 
    {
        OwnerUnit = from;
        Targets = atkTo;
    }
    public abstract void OnAdd();
    public abstract void GetTargets();
    public abstract void CombTargets();
    public abstract void AddEffectAfterComb();
    public abstract void OnRemove();
}
