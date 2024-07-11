using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public int Hp, MaxHp, Atk, Shield, Fire, Water, Wind, Thunder, Earth, Cell,Speed,ID;
    public int TotalAtkCount = 1;
    public int RemainAtkCount = 1;
    public int TotalCombCount = 1;
    public int RemainCombCount = 1;
    public Skill Unique,NormalAtk,Comb;
    public List<string> CombTypes = new();
    [SerializeField]
    public List<Buff> Buffs = new();
    public bool isBoss, isDead, hvaeComb;
    public Animator Animator;
    public Image HpBar,Icon;
    public Transform CloseAtkPos,StartParent;
    public CanvasGroup CanvasGroup;
    private EventSystem _EventSystem;
    private GraphicRaycaster gra;
    public Unit target;
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
    }

    public void UpdateHpbar()
    {
        //c# int1/int3 = 0
        HpBar.fillAmount = (float)Hp / (float)MaxHp;
    }

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

        print("msg " + unit1.name + "当前位置: " + unit1.Cell);
        print("msg " + unit2.name + "当前位置: " + unit2.Cell);
    }

    private List<RaycastResult> GraphicRaycaster(Vector2 pos)
    {
        var mPointerEventData = new PointerEventData(_EventSystem);

        mPointerEventData.position = pos;

        List<RaycastResult> results = new List<RaycastResult>();

        gra.Raycast(mPointerEventData, results);

        return results;
    }

    public void TakeDamage(int damage)
    {
        //添加各种减伤效果
        BattleMgr.Ins.ShowDamage(this, damage.ToString());
        Hp -= damage;
        if (Hp <= 0) UnitDead();
    }

    public void UnitMove(int TargetCell)
    {
        Transform moveParent = GameObject.Find("ForMove").transform;
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
        moveParent.position = Our.transform.GetChild(Cell - 1).position;
        transform.SetParent(moveParent);
        moveParent.DOMove(Our.transform.GetChild(TargetCell - 1).position, 1f).OnComplete(
            () => 
            {
                transform.SetParent(Our.transform.GetChild(TargetCell - 1));
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
    }

    public void Reload()
    {
        RemainAtkCount = TotalAtkCount;
        RemainCombCount = TotalCombCount;
    }
    public void OnFinishAtk()
    {

        print("msgOnFinishAtk");
        StartCoroutine(BattleMgr.Ins.PlayFirsrtAnimInQueue());
    }

    public void UnitDead()
    {
        if (!CompareTag("Our"))
        {
            BattleMgr.Ins.Enemys.Remove(this);
        }
        else
        {
            BattleMgr.Ins.Team.Remove(this);
        }
        foreach (var item in BattleMgr.Ins.AnimQueue)
        {
            if (item.Split(":")[0] == ID.ToString())
            {

            }
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
        //如果一个阵营死完 战斗结束
        if (BattleMgr.Ins.Enemys.Count == 0 || BattleMgr.Ins.Team.Count == 0)
        {
            BattleMgr.Ins.ExitBattle();
        }
        Destroy(gameObject);
        //throw new Exception("msg " + name + " Dead!");
    }
    public void ShowSkillName(string SkillName)
    {
        BattleMgr.Ins.ShowSkillName(this,SkillName);
    }
    public abstract void ExecuteAtk();
    public abstract void AddAtkEffect();//在动画帧攻击之后调用
    public abstract void ExecuteComb();
    public abstract void CaculDamageOnAtk();
    public abstract void CaculDamageOnComb();
    public abstract void CaculDamageOnAtked(int DamageValueFromAtker);
    public abstract void OnTurnEnd();
}
