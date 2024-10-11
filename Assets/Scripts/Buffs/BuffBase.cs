using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BuffBase
{
    public string Name, Dscrp;
    public Unit BuffFrom, BuffTo;
    public int stack;
    public virtual void OnTurnEnd()
    {
        stack -=1;
        if (stack == 0)
        {
            BuffTo.Buffs.Remove(this);
        }
    }
}
