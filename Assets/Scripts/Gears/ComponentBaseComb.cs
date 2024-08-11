using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBaseComb: ComponentBase
{
    public int RemainCombCount, TotalCombCount;
    public List<string> CombTypes = new();

    public void Init(string[] Data,Unit u)
    {
        base.Init(Data);
        OwnerUnit = u;
        Type = "追打";
        string[] combtypes = Data[4].Split('&');
        foreach (string combtype in combtypes)
        {
            CombTypes.Add(combtype);
        }
    }
    public virtual void CombTargets()
    {
        //追打目标
    }
}
