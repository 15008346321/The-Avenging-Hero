using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class Unit : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public int Cell,Damage,AtkCountMax,AtkCountCurr;
    public float Hp, MaxHp, Atk, Shield, Speed;
    public string Element;
    public bool isBoss, isDead, 致盲, 麻痹,IsEnemy,IsEnterUnitMove;
    public List<ComponentBaseBuff> Buffs = new();
    public string[] Tags = new string[4];

    public TextMeshProUGUI TMP,SpeedTMP;
    public Animator Animator;
    public Image HpBar, Icon;
    public Transform StartParent,FontPos,RunPosParent;
    public RectTransform TMPNameNode;
    public EventSystem _EventSystem;
    public GraphicRaycaster gra;
    public UnitData OriData;
    public Action FinishAtkAction, FinishBehaviorAction;

    public void Awake()
    {
        _EventSystem = FindObjectOfType<EventSystem>();
        gra = FindObjectOfType<GraphicRaycaster>();
        FinishAtkAction += OnFinishAtk;
        HpBar = transform.Find("Canvas/HpBar").GetComponent<Image>();
        TMP = transform.Find("Canvas/TMP").GetComponent<TextMeshProUGUI>();
        SpeedTMP = transform.Find("Canvas/Speed/TMP").GetComponent<TextMeshProUGUI>();

        FontPos = transform.Find("Canvas/FontPos");
    }

    #region====初始化方法
    public void Init(UnitData data)
    {
        Hp = MaxHp = data.MaxHp;
        Atk = data.Atk;
        Speed = data.Speed;
        AtkCountMax = 1;
    }
   
    #endregion
    #region ====拖动方法
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartParent = transform.parent;
        if (EventsMgr.Ins.IsMoveToStatue)
        {

        }
        else
        {
            BattleMgr.Ins.SetPosSlotAlpha(0.4f);

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Animator.enabled = false;
        TMP.raycastTarget = false;
        //100是Canvas.planeDistance
        Vector3 globalMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 100));
        // 设置对象的位置  
        transform.position = globalMousePos;
        //神像时
        if (EventsMgr.Ins.IsMoveToStatue)
        {
        }
        //战斗时
        else
        {
            BattleMgr.Ins.SetPosSlotAlpha(0.4f);
            if (eventData.pointerEnter.CompareTag("Pos"))
            {
                eventData.pointerEnter.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }

        //用tmp判断 路劲结构:slot/Unit/canvas/tmp
        if (eventData.pointerEnter.transform.parent.parent.CompareTag("Our"))
        {
            //设置slot的alpha
            eventData.pointerEnter.transform.parent.parent.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        BattleMgr.Ins.SetPosSlotAlpha(0f);


            //print(item.gameObject.name);
            //if (item.gameObject.transform.CompareTag("Our"))
            //{
            //    ChangePos(transform.GetComponent<Unit>(), item.gameObject.transform.parent.GetComponent<Unit>());
            //    transform.SetParent(item.gameObject.transform.parent.parent);
            //    transform.localPosition = Vector2.zero;
            //    item.gameObject.transform.parent.SetParent(StartParent);
            //    item.gameObject.transform.parent.localPosition = Vector2.zero;
            //}
        if (eventData.pointerEnter.CompareTag("Pos"))
        {
            transform.SetParent(eventData.pointerEnter.transform);
            OriData.Cell = Cell = int.Parse(transform.parent.name);
            transform.localPosition = Vector2.zero;
        }
        else if (eventData.pointerEnter.CompareTag("Pray"))
        {
            transform.SetParent(eventData.pointerEnter.transform);
            transform.localPosition = Vector2.zero;
            EventsMgr.Ins.UnitDatas[transform.parent.GetSiblingIndex()] = OriData;
        }
        else if (eventData.pointerEnter.CompareTag("Our"))
        {
            (eventData.pointerEnter.transform.parent.parent.parent, transform.parent) = (transform.parent, eventData.pointerEnter.transform.parent.parent.parent);
            transform.localPosition = Vector2.zero;
            eventData.pointerEnter.transform.parent.parent.transform.localPosition = Vector2.zero;
            if (transform.parent.CompareTag("Pray"))
            {
                EventsMgr.Ins.UnitDatas[transform.parent.GetSiblingIndex()] = OriData;
            }
        }
        else
        {
            transform.localPosition = Vector2.zero;
        }
        TMP.raycastTarget = true;
        Animator.enabled = true;
    }

    private void ChangePos(Unit unit1, Unit unit2)
    {
        (unit2.Cell, unit1.Cell) = (unit1.Cell, unit2.Cell);
        (unit2.OriData.Cell, unit1.OriData.Cell) = (unit1.OriData.Cell, unit2.OriData.Cell);
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
        IsEnterUnitMove = true;
        GameObject Our;
        GameObject Ene;
        if (!IsEnemy)
        {
            Our = BattleMgr.Ins.ourObj;
            Ene = BattleMgr.Ins.eneObj;
        }
        else
        {
            Our = BattleMgr.Ins.eneObj;
            Ene = BattleMgr.Ins.ourObj;
        }
        Animator.enabled = false;
        transform.DOMove(Our.transform.GetChild(TargetCell - 1).position, 1f).OnComplete(
            () => 
            {
                transform.SetParent(Our.transform.GetChild(TargetCell - 1));
                Cell = TargetCell;
                Animator.enabled = true;
                BattleMgr.Ins.FindNextActionUnit();
            }
        );
    }

    internal void MoveToEnemyFrontRow()
    {
        IsEnterUnitMove = false;
        GameObject Our;
        GameObject Ene;
        if (!IsEnemy)
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
            if (Our.transform.GetChild(0).childCount == 0 
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
        if (!IsEnterUnitMove)
        {
            FindNextActionUnit();
        }

    }
    #endregion
    #region====流程方法
    public void ReloadAtk()
    {
        AtkCountCurr = AtkCountMax;
    }
    public void FindNextActionUnit()
    {
        BattleMgr.Ins.FindNextActionUnit();
    }
    #endregion
    #region====单位通用方法
    public virtual void GetTargets()
    {
        BattleMgr.Ins.Targets.Clear();

        GameObject SearchFor;
        if (!IsEnemy) { SearchFor = BattleMgr.Ins.eneObj; }
        else { SearchFor = BattleMgr.Ins.ourObj; }

        Unit u = null;

        // 根据Cell的值确定要搜索的行  
        int[] rowIndices;
        if (new[] { 1, 4, 7 }.Contains(Cell))
        {
            rowIndices = new[] { 0, 3, 6 }; // 第一排  
        }
        else if (new[] { 2, 5, 8 }.Contains(Cell))
        {
            rowIndices = new[] { 1, 4, 7 }; // 第二排  
        }
        else
        {
            rowIndices = new[] { 2, 5, 8 }; // 第三排  
        }

        // 遍历行索引  
        foreach (int index in rowIndices)
        {
            if (SearchFor.transform.GetChild(index).childCount > 0)
            {
                u = SearchFor.transform.GetChild(index).GetChild(0).GetComponent<Unit>();
                if (u != null)
                {
                    BattleMgr.Ins.Targets.Add(u);
                    break; // 找到后跳出循环  
                }
            }
        }
    }
    //public void CheckComb(string currDebuff)
    //{
    //    if (BattleMgr.Ins.MainTarget.isDead) return;
    //    List<Unit> units;
    //    if (!IsEnemy)
    //    {
    //        units = BattleMgr.Ins.Team;
    //    }
    //    else
    //    {
    //        units = BattleMgr.Ins.Enemys;
    //    }
    //    foreach (var item in units)
    //    {
    //        if (item.Comb. CombTypes.Contains(currDebuff) 
    //            && item.Comb.RemainCombCount > 0 
    //            && !item.麻痹)
    //        {
    //            BattleMgr.Ins.AnimQueue.Insert(0, item.ID + ":Comb");
    //            item.Comb.RemainCombCount -= 1;
    //            break;
    //        }
    //    }
    //}
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
        //if (r == 0)
        //{
        //    Fire += value;
        //    BattleMgr.Ins.ShowFont(this, "火属性+" + value);
        //}
        //else if (r == 1)
        //{
        //    Water += value;
        //    BattleMgr.Ins.ShowFont(this, "水属性+" + value);
        //}
        //else if (r == 2)
        //{
        //    Wind += value;
        //    BattleMgr.Ins.ShowFont(this, "风属性+" + value);
        //}
        //else if (r == 3)
        //{
        //    Thunder += value;
        //    BattleMgr.Ins.ShowFont(this, "雷属性+" + value);
        //}
        //else if (r == 4)
        //{
        //    Earth += value;
        //    BattleMgr.Ins.ShowFont(this, "土属性+" + value);
        //}
    }
#region====战斗方法
    public void ExecuteAtk()
    {
        GetTargets();

        if (BattleMgr.Ins.Targets.Count == 0)//没有目标就走位 然后进行下一个
        {
            MoveToEnemyFrontRow();
        }
        else
        {
            if (致盲)
            {
                BattleMgr.Ins.ShowFont(this, "致盲-行动失败");
                BattleMgr.Ins.FindNextActionUnit();
                return;
            }
            Animator.Play("atk");//后面两方法在动画帧后段调用
        }
    }
   
    //输出的攻击伤害 动画上调用
    public void CaculDamageOnAtk()
    {
        BattleMgr.Ins.CaculDamage(this);
    }
    //在动画帧攻击之后调用 加攻击时特效(Debuff 攻击养成等) 动画上调用
    public void AddAtkEffectOnAtk()
    {
        //
    }
    //public void ExecuteComb() 
    //{
    //    Comb.GetTargets();
    //    if (BattleMgr.Ins.MainTarget == null) return;
    //    Animator.Play("closeAtk2");
    //}
    ////输出的追打伤害
    //public void CaculDamageOnComb()
    //{
    //    Comb.CombTargets();
    //}
    ////在动画帧攻击之后调用 加追打时特效(Debuff 追打养成等)
    //public void AddAtkEffectOnComb()
    //{
    //    Comb.OnComb();
    //}
    //AttrType:Atk/Fire/Water/Wind/Thunder/Earth  AtkType:Atk/Comb
    public void TakeAtkDamage(Unit DamageFrom, float rate, string AttrType = "Atk", string AtkType = "Atk")
    {
        float damageReduce = 1, damageRate = 1;
        //switch (AttrType)
        //{
        //    case "Fire":
        //        damageReduce = DamageFrom.Fire / (DamageFrom.Fire + Fire);
        //        if (Element == "风") damageRate = 1.5f;
        //        Damage = Mathf.RoundToInt(DamageFrom.Fire * damageReduce * damageRate);
        //        break;
        //    case "Water":
        //        damageReduce = DamageFrom.Water / (DamageFrom.Water + Water);
        //        if (Element == "火") damageRate = 1.5f;
        //        Damage = Mathf.RoundToInt(DamageFrom.Water * damageReduce * damageRate);
        //        break;
        //    case "Wind":
        //        damageReduce = DamageFrom.Wind / (DamageFrom.Wind + Wind);
        //        if (Element == "土") damageRate = 1.5f;
        //        Damage = Mathf.RoundToInt(DamageFrom.Wind * damageReduce * damageRate);
        //        break;
        //    case "Thunder":
        //        damageReduce = DamageFrom.Thunder / (DamageFrom.Thunder + Thunder);
        //        if (Element == "水") damageRate = 1.5f;
        //        Damage = Mathf.RoundToInt(DamageFrom.Thunder * damageReduce * damageRate);
        //        break;
        //    case "Earth":
        //        damageReduce = DamageFrom.Earth / (DamageFrom.Earth + Earth);
        //        if (Element == "雷") damageRate = 1.5f;
        //        Damage = Mathf.RoundToInt(DamageFrom.Earth * damageReduce * damageRate);
        //        break;
        //    default:
        //        damageReduce = DamageFrom.Atk / (DamageFrom.Atk + Atk);
        //        Damage = Mathf.RoundToInt(DamageFrom.Atk * damageReduce);
        //        break;
        //}
        //Damage = Mathf.RoundToInt(Damage * rate);
        //if(AtkType == "Atk") RcAtk();
        //if(AtkType == "Comb") RcComb();
        //if (AttrType == "Atk") RcAtk();
        //if (AttrType == "Fire") RcFire();
        //if (AttrType == "Water") RcComb();
        //if (AttrType == "Wind") RcAtk();
        //if (AttrType == "Thunder") RcComb();
        //if (AttrType == "Earth") RcComb();

        BattleMgr.Ins.ShowFont(this, Damage, "Hurt");
    }//被攻击时特效 减伤 养成

    public void RcAtk()
    {
        foreach (var item in Buffs)
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
    }
    public void RcPhs()
    {
        foreach (var item in Buffs)
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
    }
    public void RcWater()
    {
        foreach (var item in Buffs)
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
    }
    public void RcThunder()
    {
        foreach (var item in Buffs)
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
    }
    public void OnBattleStart()
    {
    }
    public void OnTurnEnd()
    {
        致盲 = false;
        麻痹 = false;
        for (int i = 0; i < Buffs.Count; i++)
        {
            Buffs[i].OnTurnEnd();
        }
    }

    public virtual void OnTurnStart()
    {

    }
    #endregion

    public void OnFinishAtk()
    {
        Animator.Play("idle");
        BattleMgr.Ins.FindNextActionUnit();
    }
}
