using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class Unit : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public int Cell,ID,Damage;
    public float Hp, MaxHp, Atk, Shield, Fire, Water, Wind, Thunder, Earth, Speed;
    public string Element;
    public bool isBoss, isDead, 致盲, 麻痹;
    public ComponentBaseAtk NormalAtk;
    public ComponentBaseComb Comb;
    public ComponentBaseWeapon Weapon;
    public ComponentBaseArmor Armor;
    public ComponentBaseSupport Support;
    public List<ComponentBase> Components;
    public List<ComponentBaseBuff> Buffs = new();

    public Animator Animator;
    public Image HpBar, Icon;
    public Transform CloseAtkPos,StartParent;
    public RectTransform TMPNameNode;
    public CanvasGroup CanvasGroup;
    public EventSystem _EventSystem;
    public GraphicRaycaster gra;
    public UnitData OriData;

    public void Awake()
    {
        _EventSystem = FindObjectOfType<EventSystem>();
        gra = FindObjectOfType<GraphicRaycaster>();
    }
    #region====初始化方法
    public void TeamInitAttr(UnitData data)
    {
        name = data.Name;
        //Icon.sprite = data.sprite;
        NormalAtk = Activator.CreateInstance(Type.GetType(data.AtkName)) as ComponentBaseAtk;
        NormalAtk.Init(CSVManager.Ins.Atks[data.AtkName], this);
        Comb = Activator.CreateInstance(Type.GetType(data.CombName)) as ComponentBaseComb;
        Comb.Init(CSVManager.Ins.Combs[data.CombName],this);

        if (data.WeaponName != null)
        {
            Weapon = Activator.CreateInstance(Type.GetType(data.WeaponName)) as ComponentBaseWeapon;
            Components.Add(Weapon);
        }
        if (data.ArmorName != null)
        {
            Armor = Activator.CreateInstance(Type.GetType(data.ArmorName)) as ComponentBaseArmor;
            Components.Add(Armor);
        }
        if (data.SupportName != null)
        {
            Support = Activator.CreateInstance(Type.GetType(data.SupportName)) as ComponentBaseSupport;
            Components.Add(Support);
        }

        Hp = MaxHp = data.MaxHp;
        Atk = data.Atk;
        Fire = data.Fire;
        Water = data.Water;
        Wind = data.Wind;
        Thunder = data.Thunder;
        Earth = data.Earth;
        Speed = data.Speed;

        SetElement();
    }

    public void EnemyInitAttr(string Mname)
    {
        //0id 1名称2最大生命值3攻击4火5水6风7雷8土9特性10普攻11追打12被动
        string[] data = CSVManager.Ins.Units[Mname];
        name = Mname;
        //根据单位拥有的技能名字，从所有追打中检索出来加入该单位追打
        //0id 1名称2类型3效果4价格5code6追打状态7造成状态
        //NormalAtk = new Skill(CSVManager.Ins.全技能表[属性[9]]);
        //if (属性[10] != "") Comb = new Skill(CSVManager.Ins.全技能表[属性[10]]);
        //if (属性[11] != "") Special = new Skill(CSVManager.Ins.全技能表[属性[11]]);

        int result;
        Hp      = int.Parse(data[2]);
        MaxHp   = int.TryParse(data[2], out result) ? result : 0;
        Atk     = int.TryParse(data[3], out result) ? result : 0;
        Fire    = int.TryParse(data[4], out result) ? result : 0;
        Water   = int.TryParse(data[5], out result) ? result : 0;
        Wind    = int.TryParse(data[6], out result) ? result : 0;
        Thunder = int.TryParse(data[7], out result) ? result : 0;
        Earth   = int.TryParse(data[8], out result) ? result : 0;
        Speed   = int.Parse(data[9]);

        SetElement();

        NormalAtk = Activator.CreateInstance(Type.GetType(data[10])) as ComponentBaseAtk;
        NormalAtk.Init(CSVManager.Ins.Atks[data[10]], this);
        Comb = Activator.CreateInstance(Type.GetType(data[11])) as ComponentBaseComb;
        Comb.Init( CSVManager.Ins.Combs[data[11]], this);

        //AttrInfo.Instance.ShowInfo(this);
        //AP.Play("idle");
    }

    public void SetElement()
    {
        float a = 0;
        if (Atk > a) { a = Atk; Element = "物理"; }
        if (Fire > a) { a = Fire; Element = "火"; }
        if (Water > a) { a = Water; Element = "水"; }
        if (Wind > a) { a = Wind; Element = "风"; }
        if (Thunder > a) { a = Thunder; Element = "雷"; }
        if (Earth > a) { a = Earth; Element = "土"; }
    }
   
    #endregion
    #region ====拖动方法
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartParent = transform.parent;
        //CanvasGroup.blocksRaycasts = false;
        BattleMgr.Ins.SetPosSlotAlpha(0.4f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if (eventData.pointerEnter.CompareTag("Pos"))
        {
            BattleMgr.Ins.SetPosSlotAlpha(0.4f);
            eventData.pointerEnter.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (eventData.pointerEnter.transform.parent.CompareTag("Our"))
        {
            BattleMgr.Ins.SetPosSlotAlpha(0.4f);
            eventData.pointerEnter.transform.parent.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        BattleMgr.Ins.SetPosSlotAlpha(0f);

        var list = GraphicRaycaster(Input.mousePosition);

        foreach (var item in list)
        {
            if (item.gameObject.transform.parent.CompareTag("Our"))
            {
                ChangePos(transform.GetComponent<Unit>(), item.gameObject.transform.parent.GetComponent<Unit>());
                transform.SetParent(item.gameObject.transform.parent.parent);
                transform.localPosition = Vector2.zero;
                item.gameObject.transform.parent.SetParent(StartParent);
                item.gameObject.transform.parent.localPosition = Vector2.zero;
            }
            else if (item.gameObject.CompareTag("Pos"))
            {
                print("enter slot");
                transform.SetParent(item.gameObject.transform);
                transform.GetComponent<Unit>().Cell = int.Parse(transform.parent.name);
                transform.GetComponent<Unit>().OriData.Cell = int.Parse(transform.parent.name);
                print("msg " + transform.name + "当前位置: " + transform.GetComponent<Unit>().Cell);
                transform.localPosition = Vector2.zero;
            }
            else
            {
                print("enter null");
                transform.localPosition = Vector2.zero;
            }
        }
        //CanvasGroup.blocksRaycasts = true;
    }

    private void ChangePos(Unit unit1, Unit unit2)
    {
        (unit2.Cell, unit1.Cell) = (unit1.Cell, unit2.Cell);
        (unit2.OriData.Cell, unit1.OriData.Cell) = (unit1.OriData.Cell, unit2.OriData.Cell);
        print("msg " + unit1.name + "当前位置: " + unit1.Cell);
        print("msg " + unit2.name + "当前位置: " + unit2.Cell);
    }

    private List<RaycastResult> GraphicRaycaster(Vector2 pos)
    {
        var mPointerEventData = new PointerEventData(_EventSystem);

        mPointerEventData.position = pos;

        List<RaycastResult> results = new();

        gra.Raycast(mPointerEventData, results);

        return results;
    }
    #endregion
    #region ====战斗中移动
    public void UnitMove(int TargetCell)
    {
        Transform moveParent = transform.parent;
        GameObject Our;
        GameObject Ene;
        if (CompareTag("Our"))
        {
            Our = BattleMgr.Ins.ourObj;
            Ene = BattleMgr.Ins.eneObj;
        }
        else
        {
            Our = BattleMgr.Ins.eneObj;
            Ene = BattleMgr.Ins.ourObj;
        }
        moveParent.DOMove(Our.transform.GetChild(TargetCell - 1).position, 1f).OnComplete(
            () => 
            {
                moveParent.SetParent(Our.transform.GetChild(TargetCell - 1));
                Cell = TargetCell;
             }
        );
    }

    internal void MoveToEnemyFrontRow()
    {
        GameObject Our;
        GameObject Ene;
        if (CompareTag("Our"))
        {
            Our = BattleMgr.Ins.ourObj;
            Ene = BattleMgr.Ins.eneObj;
        }
        else
        {
            Our = BattleMgr.Ins.eneObj;
            Ene = BattleMgr.Ins.ourObj;
        }
        //GetChild(Cell) 1位置移动到2 2cell的index是1
        if (Cell == 1)
        {
            //2有空去2
            if (Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(2);
            }
            //4有空去4
            else if (Our.transform.GetChild(3).childCount == 0)
            {
                UnitMove(4);
            }
        }
        else if (Cell == 3)
        {
            //2有空去2
            if (Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(2);
            }
            //6有空去6
            else if (Our.transform.GetChild(5).childCount == 0)
            {
                UnitMove(6);
            }
        }
        else if (Cell == 7)
        {
            //8有空去8
            if (Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(8);
            }
            //4有空去4
            else if (Our.transform.GetChild(3).childCount == 0)
            {
                UnitMove(4);
            }
        }
        else if (Cell == 9)
        {
            //8有空去8
            if (Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(8);
            }
            //6有空去6
            else if (Our.transform.GetChild(5).childCount == 0)
            {
                UnitMove(6);
            }
        }
        else if (Cell == 4)
        {
            //5有空去5
            if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
            //12有空 移动到1
            else if (Our.transform.GetChild(0).childCount == 0 && Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(1);
            }
            //78有空 移动到7
            else if (Our.transform.GetChild(6).childCount == 0 && Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(7);
            }
        }
        else if (Cell == 6)
        {
            //5有空去5
            if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
            //23有空 移动到3
            if (Our.transform.GetChild(2).childCount == 0 && Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(3);
            }
            //89有空 移动到9
            else if (Our.transform.GetChild(8).childCount == 0 && Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(9);
            }
        }
        else if (Cell == 2)
        {
            //1有空 上路有敌人去1
            if (Our.transform.GetChild(1).childCount == 0 
                && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(1);
            }
            //3有空下路有敌人 去3
            else if (Our.transform.GetChild(2).childCount == 0 
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(3);
            }
            //上面都不满足 5有空去5
            else if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
        }
        else if (Cell == 8)
        {
            //7有空 上路有敌人去7
            if (Our.transform.GetChild(6).childCount == 0
                && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(7);
            }
            //9有空下路有敌人 去9
            else if (Our.transform.GetChild(8).childCount == 0
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(9);
            }
            //上面都不满足 5有空去5
            else if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
        }
        else if (Cell == 5)
        {
            //4有空 上路有敌人去4
            if (Our.transform.GetChild(3).childCount == 0 && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(4);
            }
            //6有空 下路有敌人去6
            else if (Our.transform.GetChild(5).childCount == 0 && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(6);
            }
            //4没空 上路有敌人 12有空 去2
            else if (Our.transform.GetChild(3).childCount > 0 && Our.transform.GetChild(0).childCount == 0 && Our.transform.GetChild(1).childCount == 0
                &&(Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(2);
            }
            //4没空 上路有敌人 78有空 去8
            else if (Our.transform.GetChild(3).childCount > 0 && Our.transform.GetChild(8).childCount == 0 && Our.transform.GetChild(7).childCount == 0
                && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(8);
            }
            //6没空 下路有敌人 23有空 去2
            else if (Our.transform.GetChild(5).childCount > 0 && Our.transform.GetChild(2).childCount == 0 && Our.transform.GetChild(1).childCount == 0
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(2);
            }
            //6没空 下路有敌人 89有空 去8
            else if (Our.transform.GetChild(5).childCount > 0 && Our.transform.GetChild(8).childCount == 0 && Our.transform.GetChild(7).childCount == 0
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(8);
            }
        }
        else
        {
            BattleMgr.Ins.ShowFont(this, "行动失败");
        }
    }
    #endregion
    #region====流程方法
    public void Reload()
    {

        print("relaod");
        NormalAtk.RemainAtkCount = NormalAtk.TotalAtkCount;
        Comb.RemainCombCount = Comb.TotalCombCount;
    }
    public void FindNextActionUnit()
    {
        BattleMgr.Ins.FindNextActionUnit();
    }
    #endregion
    #region====单位通用方法
    public Unit GetOppositeTarget()
    {
        GameObject SearchFor;
        if (CompareTag("Our")) { SearchFor = BattleMgr.Ins.eneObj; }
        else { SearchFor = BattleMgr.Ins.ourObj; }
        if (new[] { 1, 4, 7 }.Contains(Cell))
        {
            //受击单位第一排
            if (SearchFor.transform.GetChild(0).childCount > 0) return SearchFor.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(3).childCount > 0) return SearchFor.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(6).childCount > 0) return SearchFor.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<Unit>();
        }
        else if (new[] { 2, 5, 8 }.Contains(Cell))
        {
            //第二排
            if (SearchFor.transform.GetChild(1).childCount > 0) return SearchFor.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(4).childCount > 0) return SearchFor.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(7).childCount > 0) return SearchFor.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<Unit>();
        }
        else
        {
            //第三排
            if (SearchFor.transform.GetChild(2).childCount > 0) return SearchFor.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(5).childCount > 0) return SearchFor.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(8).childCount > 0) return SearchFor.transform.GetChild(8).GetChild(0).GetChild(0).GetComponent<Unit>();
        }
        return null;
    }
    public void CheckComb(string currDebuff)
    {
        if (BattleMgr.Ins.MainTarget.isDead) return;
        List<Unit> units;
        if (CompareTag("Our"))
        {
            units = BattleMgr.Ins.Team;
        }
        else
        {
            units = BattleMgr.Ins.Enemys;
        }
        foreach (var item in units)
        {
            if (item.Comb. CombTypes.Contains(currDebuff) 
                && item.Comb.RemainCombCount > 0 
                && !item.麻痹)
            {
                BattleMgr.Ins.AnimQueue.Insert(0, item.ID + ":Comb");
                item.Comb.RemainCombCount -= 1;
                break;
            }
        }
    }
    public void UpdateHpBar()
    {
        //c# int 1/int 3 = 0
        HpBar.fillAmount = Hp / MaxHp;
    }
    
    public void ShowSkillName(string SkillName)
    {
        BattleMgr.Ins.ShowSkillName(this,SkillName);
    }
    
    #endregion
    public void GetRandomMagic(int value)
    {
        var r = UnityEngine.Random.Range(0, 5);
        if (r == 0)
        {
            Fire += value;
            BattleMgr.Ins.ShowFont(this, "火属性+" + value);
        }
        else if (r == 1)
        {
            Water += value;
            BattleMgr.Ins.ShowFont(this, "水属性+" + value);
        }
        else if (r == 2)
        {
            Wind += value;
            BattleMgr.Ins.ShowFont(this, "风属性+" + value);
        }
        else if (r == 3)
        {
            Thunder += value;
            BattleMgr.Ins.ShowFont(this, "雷属性+" + value);
        }
        else if (r == 4)
        {
            Earth += value;
            BattleMgr.Ins.ShowFont(this, "土属性+" + value);
        }
    }
    #region====战斗方法
    public void ExecuteAtk()
    {
        NormalAtk.GetTargets();

        if (BattleMgr.Ins.Targets.Count == 0)//没有目标就走位 然后进行下一个
        {
            MoveToEnemyFrontRow();
            BattleMgr.Ins.FindNextActionUnit();
        }
        else
        {
            if (致盲)
            {
                BattleMgr.Ins.ShowFont(this, "致盲-行动失败");
                BattleMgr.Ins.FindNextActionUnit();
                return;
            }
            Animator.Play("closeAtk1");//后面两方法在动画帧后段调用
        }
    }
   
    //输出的攻击伤害
    public void CaculDamageOnAtk()
    {
        NormalAtk.AtkTargets();
    }
    //在动画帧攻击之后调用 加攻击时特效(Debuff 攻击养成等)
    public void AddAtkEffectOnAtk()
    {
        NormalAtk.OnAtk();
    }
    public void ExecuteComb() 
    {
        Comb.GetTargets();
        if (BattleMgr.Ins.MainTarget == null) return;
        Animator.Play("closeAtk2");
    }
    //输出的追打伤害
    public void CaculDamageOnComb()
    {
        Comb.CombTargets();
    }
    //在动画帧攻击之后调用 加追打时特效(Debuff 追打养成等)
    public void AddAtkEffectOnComb()
    {
        Comb.OnComb();
    }
    //AttrType:Atk/Fire/Water/Wind/Thunder/Earth  AtkType:Atk/Comb
    public void TakeAtkDamage(Unit DamageFrom, float rate, string AttrType = "Atk", string AtkType = "Atk")
    {
        float damageReduce = 1, damageRate = 1;
        switch (AttrType)
        {
            case "Fire":
                damageReduce = DamageFrom.Fire / (DamageFrom.Fire + Fire);
                if (Element == "风") damageRate = 1.5f;
                Damage = Mathf.RoundToInt(DamageFrom.Fire * damageReduce * damageRate);
                break;
            case "Water":
                damageReduce = DamageFrom.Water / (DamageFrom.Water + Water);
                if (Element == "火") damageRate = 1.5f;
                Damage = Mathf.RoundToInt(DamageFrom.Water * damageReduce * damageRate);
                break;
            case "Wind":
                damageReduce = DamageFrom.Wind / (DamageFrom.Wind + Wind);
                if (Element == "土") damageRate = 1.5f;
                Damage = Mathf.RoundToInt(DamageFrom.Wind * damageReduce * damageRate);
                break;
            case "Thunder":
                damageReduce = DamageFrom.Thunder / (DamageFrom.Thunder + Thunder);
                if (Element == "水") damageRate = 1.5f;
                Damage = Mathf.RoundToInt(DamageFrom.Thunder * damageReduce * damageRate);
                break;
            case "Earth":
                damageReduce = DamageFrom.Earth / (DamageFrom.Earth + Earth);
                if (Element == "雷") damageRate = 1.5f;
                Damage = Mathf.RoundToInt(DamageFrom.Earth * damageReduce * damageRate);
                break;
            default:
                damageReduce = DamageFrom.Atk / (DamageFrom.Atk + Atk);
                Damage = Mathf.RoundToInt(DamageFrom.Atk * damageReduce);
                break;
        }
        Damage = Mathf.RoundToInt(Damage * rate);
        if(AtkType == "Atk") RcAtk();
        if(AtkType == "Comb") RcComb();
        if (AttrType == "Atk") RcAtk();
        if (AttrType == "Fire") RcFire();
        if (AttrType == "Water") RcComb();
        if (AttrType == "Wind") RcAtk();
        if (AttrType == "Thunder") RcComb();
        if (AttrType == "Earth") RcComb();

        BattleMgr.Ins.ShowFont(this, Damage, "Hurt");
    }//被攻击时特效 减伤 养成

    #region ==== 受击特效 ====
    public void RcAtk()
    {
        foreach (var item in Buffs)
        {
            item.RcAtk();
        }
        foreach (var item in Components)
        {
            item.RcAtk();
        }
    }

    public void RcComb()
    {
        foreach (var item in Buffs)
        {
            item.RcComb();
        }
        foreach (var item in Components)
        {
            item.RcComb();
        }
    }
    public void RcPhs()
    {
        foreach (var item in Buffs)
        {
            item.RcPhs();
        }
        foreach (var item in Components)
        {
            item.RcPhs();
        }
    }
    public void RcFire()
    {
        foreach (var item in Buffs)
        {
            item.RcFire();
        }
        foreach (var item in Components)
        {
            item.RcFire();
        }
    }
    public void RcWater()
    {
        foreach (var item in Buffs)
        {
            item.RcWater();
        }
        foreach (var item in Components)
        {
            item.RcWater();
        }
    }
    public void RcWind()
    {
        foreach (var item in Buffs)
        {
            item.RcWind();
        }
        foreach (var item in Components)
        {
            item.RcWind();
        }
    }
    public void RcThunder()
    {
        foreach (var item in Buffs)
        {
            item.RcThunder();
        }
        foreach (var item in Components)
        {
            item.RcThunder();
        }
    }
    public void RcEarth()
    {
        foreach (var item in Buffs)
        {
            item.RcEarth();
        }
        foreach (var item in Components)
        {
            item.RcEarth();
        }
    }
    #endregion
    public void OnTurnEnd()
    {
        致盲 = false;
        麻痹 = false;
        for (int i = 0; i < Buffs.Count; i++)
        {
            Buffs[i].OnTurnEnd();
        }
    }
    #endregion
}
