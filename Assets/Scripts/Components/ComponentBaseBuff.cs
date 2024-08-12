using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBaseBuff: ComponentBase
{
    public int MaxStack,CurrStack;
    public Unit BuffFrom;

    public virtual void OnAdd(Unit from, Unit to)
    {
        BuffFrom = from;
        OwnerUnit = to;
    }
}
