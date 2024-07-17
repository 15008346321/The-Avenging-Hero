using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Buff:MonoBehaviour
{
    public int ID;
    public int CurrStack;
    public int MaxStack;
    public string Name;
    public string Dscrp;
    public Unit BuffFrom, BuffTarget;

    public abstract void Init(Unit from, Unit target);
    public abstract void AddLayer();
    public abstract void OnAttack();
    public abstract void OnComb();
    public abstract void OnAttacked();
    public abstract void OnCombed();
    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
    public abstract void Remove();
}
