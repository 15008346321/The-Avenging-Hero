using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Unit : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public int Hp, MaxHp, Atk, Shield, Fire, Water, Wind, Thunder, Earth, Cell,Speed,ID;
    public Skill Unique,NormalAtk,Comb;
    public AtkBase AtkSkill;
    public CombBase CombSkill;
    public List<string> CombTypes = new();
    public List<BuffBase> Buffs = new();
    public bool isBoss, isDead, hvaeComb;
    public Animator Animator;
    public Image HpBar,Icon;
    public Transform CloseAtkPos,StartParent;
    public RectTransform TMPNameNode;
    public CanvasGroup CanvasGroup;
    private EventSystem _EventSystem;
    private GraphicRaycaster gra;
    public Unit target;
    public UnitData OriData;

    public bool IsBlinded
    {
        get
        {
            return Buffs?.Any(buff => buff is BuffBlind) ?? false;
        }
    }
    #region====初始化方法
    private void Awake()
    {
        _EventSystem = FindObjectOfType<EventSystem>();
        gra = FindObjectOfType<GraphicRaycaster>();
    }
    public void EnemyInitAttr(string Mname)
    {
        //0id 1名称2最大生命值3攻击4火5水6风7雷8土9特性10普攻11追打12被动
        string[] 属性 = CSVManager.Ins.模板参数[Mname];
        name = Mname;
        //根据单位拥有的技能名字，从所有追打中检索出来加入该单位追打
        //0id 1名称2类型3效果4价格5code6追打状态7造成状态
        //NormalAtk = new Skill(CSVManager.Ins.全技能表[属性[9]]);
        //if (属性[10] != "") Comb = new Skill(CSVManager.Ins.全技能表[属性[10]]);
        //if (属性[11] != "") Special = new Skill(CSVManager.Ins.全技能表[属性[11]]);

        int result;
        Hp      = int.TryParse(属性[2], out result) ? result : 0;
        MaxHp   = int.TryParse(属性[2], out result) ? result : 0;
        Atk     = int.TryParse(属性[3], out result) ? result : 0;
        Fire    = int.TryParse(属性[4], out result) ? result : 0;
        Water   = int.TryParse(属性[5], out result) ? result : 0;
        Wind    = int.TryParse(属性[6], out result) ? result : 0;
        Thunder = int.TryParse(属性[7], out result) ? result : 0;
        Earth   = int.TryParse(属性[8], out result) ? result : 0;

        //AttrInfo.Instance.ShowInfo(this);
        //AP.Play("idle");
    }
    public void TeamInitAttr(UnitData data)
    {
        name = data.Name;
        //Icon.sprite = data.sprite;
        NormalAtk = data.NormalAtk;
        Comb = data.Comb;
        Unique = data.Special;
        Hp = MaxHp = data.MaxHp;
        Atk = data.Atk;
        Fire = data.Fire;
        Water = data.Water;
        Wind = data.Wind;
        Thunder = data.Thunder;
        Earth = data.Earth;

        AtkSkill = new 斩击(this);
    }
    #endregion
    #region ====拖动方法
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartParent = transform.parent;
        CanvasGroup.blocksRaycasts = false;
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
        CanvasGroup.blocksRaycasts = true;
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
        AtkSkill.RemainAtkCount = AtkSkill.TotalAtkCount;
        CombSkill.RemainCombCount = CombSkill.TotalCombCount;
    }
    public void FindNextActionUnit()
    {
        if (BattleMgr.Ins.CheckBattleEnd())
        {
            BattleMgr.Ins.ExitBattle();
            return;
        }
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
            if (item.CombTypes.Contains(currDebuff) && item.CombSkill.RemainCombCount > 0)
            {
                BattleMgr.Ins.AnimQueue.Insert(0, item.ID + ":Comb");
                item.CombSkill.RemainCombCount -= 1;
                break;
            }
        }
    }
    public void UpdateHpBar()
    {
        //c# int 1/int 3 = 0
        HpBar.fillAmount = (float)Hp / (float)MaxHp;
    }
    //在伤害字体动画时事件调用
    public void UnitDead()
    {
        print("set" + name + " isdead true");
        isDead = true;
        if (!CompareTag("Our"))
        {
            BattleMgr.Ins.Enemys.Remove(this);
        }
        else
        {
            BattleMgr.Ins.Team.Remove(this);
        }
        //把死了就不需要执行的行动删除
        if (BattleMgr.Ins.AnimQueue.Count > 0)
        {
            for (int i = BattleMgr.Ins.AnimQueue.Count-1; i >= 0; i--)
            {
                if (BattleMgr.Ins.AnimQueue[i].Split(":")[0] == ID.ToString())
                {
                    BattleMgr.Ins.AnimQueue.RemoveAt(i);
                }
            }
        }
        BattleMgr.Ins.AllUnit.Remove(this);
    }
    public void ShowSkillName(string SkillName)
    {
        BattleMgr.Ins.ShowSkillName(this,SkillName);
    }
    
    #endregion
    public void GetRandomMagic(int value)
    {
        var r = Random.Range(0, 5);
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
    #region====单位继承方法
    public void ExecuteAtk()
    {
        BattleMgr.Ins.AtkU = this;
        AtkSkill.GetTargets();
        if (AtkSkill.Targets.Count == 0)//没有目标就走位 然后进行下一个
        {
            MoveToEnemyFrontRow();
            BattleMgr.Ins.FindNextActionUnit();
        }
        else
        {
            if (IsBlinded)
            {
                BattleMgr.Ins.ShowFont(this, "行动失败");
                BattleMgr.Ins.FindNextActionUnit();
                return;
            }
            Animator.Play("closeAtk1");//后面两方法在动画帧后段调用
        }
    }
   
    //输出的攻击伤害
    public void CaculDamageOnAtk()
    {
        AtkSkill.AtkTargets();
    }
    //在动画帧攻击之后调用 加攻击时特效(Debuff 攻击养成等)
    public void AddAtkEffectOnAtk()
    {
        AtkSkill.AddEffectAfterAtk();
    }
    public void ExecuteComb() 
    {
        BattleMgr.Ins.CombU = this;
        CombSkill.GetTargets();
        Animator.Play("closeAtk2");
    }
    //输出的追打伤害
    public void CaculDamageOnComb()
    {
        CombSkill.CombTargets();
    }
    //在动画帧攻击之后调用 加追打时特效(Debuff 追打养成等)
    public void AddAtkEffectOnComb()
    {
        CombSkill.AddEffectAfterComb();
    }
    public void TakeAtkDamage(Unit DamageFrom, float rate, string DamageType = "Atk")
    {
        int damage;
        float damageReduce = 1, damageRate = 1;
        switch (DamageType)
        {
            case "Fire":
                damageReduce = DamageFrom.Fire / (DamageFrom.Fire + Fire);
                if (DamageFrom.Fire > Fire) damageRate = 1 + (Wind / (Wind + 50));
                damage = Mathf.RoundToInt(DamageFrom.Fire * damageReduce * damageRate);
                break;
            case "Water":
                damageReduce = DamageFrom.Water / (DamageFrom.Water + Water);
                if (DamageFrom.Water > Water) damageRate = 1 + (Fire / (Fire + 50));
                damage = Mathf.RoundToInt(DamageFrom.Water * damageReduce * damageRate);
                break;
            case "Wind":
                damageReduce = DamageFrom.Wind / (DamageFrom.Wind + Wind);
                if (DamageFrom.Wind > Wind) damageRate = 1 + (Earth / (Earth + 50));
                damage = Mathf.RoundToInt(DamageFrom.Wind * damageReduce * damageRate);
                break;
            case "Thunder":
                damageReduce = DamageFrom.Thunder / (DamageFrom.Thunder + Thunder);
                if (DamageFrom.Thunder > Thunder) damageRate = 1 + (Water / (Water + 50));
                damage = Mathf.RoundToInt(DamageFrom.Thunder * damageReduce * damageRate);
                break;
            case "Earth":
                damageReduce = DamageFrom.Earth / (DamageFrom.Earth + Earth);
                if (DamageFrom.Earth > Earth) damageRate = 1 + (Thunder / (Thunder + 50));
                damage = Mathf.RoundToInt(DamageFrom.Earth * damageReduce * damageRate);
                break;
            default:
                damageReduce = DamageFrom.Atk / (DamageFrom.Atk + Atk);
                damage = Mathf.RoundToInt(DamageFrom.Atk * damageReduce);
                break;
        }
        damage = Mathf.RoundToInt(damage * rate);
        BattleMgr.Ins.ShowFont(this, damage, "Hurt");
    }//被攻击时特效 减伤 养成

    public void OnAtked(int DamageValueFromAtker)
    {

    }

    public void OnCombed(int DamageValueFromAtker)
    {

    }
    public void OnTurnEnd()
    {
        foreach (var item in Buffs)
        {
            item.OnTurnEnd();
        }
    }
    #endregion
}
