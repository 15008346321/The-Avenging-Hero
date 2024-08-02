using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CombBase
{
    public int ID;
    public string Name;
    public string Dscrp;
    public int TotalCombCount;
    public int RemainCombCount;
    public Unit OwnerUnit;
    public List<string> CombTypes = new();

    public void Init(Unit Owner, string[] Data)
    {
        OwnerUnit = Owner;
        ID = int.Parse(Data[0]);
        Name = Data[1];
        Dscrp = Data[2];
        TotalCombCount = int.Parse(Data[3]);
        CombTypes.Add(Data[4]);
    }
    public abstract void OnAdd();
    public abstract void GetTargets();
    public abstract void CombTargets();
    public abstract void AddEffectAfterComb();
    public abstract void OnRemove();
}
