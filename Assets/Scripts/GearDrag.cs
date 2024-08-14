using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GearDrag :MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject OwnerNum, GearType;
    public Transform StartParent;
    public List<Transform> GearOwnerIcon = new();
    public RectTransform RectTransform;
    public ComponentBase Gear;
    public CanvasGroup CanvasGroup;
    public EventSystem EventSystem;
    private GraphicRaycaster _GraphicRaycaster;
    public bool IsGearOrSlot;

    public void Init(string name)
    {
        //生成并初始化脚本
        Gear = Activator.CreateInstance(Type.GetType(name)) as ComponentBase;
        Gear.GearDrag = this;
        Gear.Init(CSVManager.Ins.Gears[name]);
        //拖拽监测
        RectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
        EventSystem = FindObjectOfType<EventSystem>();
        _GraphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        StartParent = transform.parent;
        transform.SetParent(BagManager.Ins.DragParent);
        OwnerNum.SetActive(false);
        GearType.SetActive(false);
        if (Gear.Type == "武器") TeamManager.Ins.ShowGearSlot(0);
        if (Gear.Type == "防具") TeamManager.Ins.ShowGearSlot(1);
        if (Gear.Type == "辅助") TeamManager.Ins.ShowGearSlot(2);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        RectTransform.anchoredPosition += eventData.delta;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        TeamManager.Ins.StopShowGearSlot();

        var list = GraphicRaycaster(Input.mousePosition);


        
        foreach (var item in list)
        {
            //检测是否在物品上 Item Slot在背包中换位置 武器防具辅助在角色面板上面换
            if (item.gameObject.tag == "Item")
            {
                //如果在物品上则执行以下代码
                //---交换位置---//
                IsGearOrSlot = true;
                RectTransform.position = item.gameObject.transform.position;
                transform.SetParent(item.gameObject.transform.parent);
                transform.localPosition = Vector2.zero;
                item.gameObject.transform.SetParent(StartParent);
                item.gameObject.transform.localPosition = Vector2.zero;
                break;
            }
            //检测是否在格子上
            else if (item.gameObject.tag == "Slot")
            {
                //如果在格子上则将isSlot设置为true方便后续代码
                IsGearOrSlot = true;
                //保存格子位置坐标以便切换位置
                transform.SetParent(item.gameObject.transform);
                transform.localPosition = Vector2.zero;
                break;
            }
            //武器防具辅助 逻辑一样
            else if (item.gameObject.tag == "WeaponSlot" && Gear.Type == "武器")
            {

                GearOwnerIcon[TeamManager.Ins.CurrMbrIdx].SetAsLastSibling();
                OwnerNum.SetActive(true);
                GearType.SetActive(true);

                //当前的单位
                UnitData ud = TeamManager.Ins.TeamData[TeamManager.Ins.CurrMbrIdx];

                if (ud.Weapon == Gear) return;

                //把当目标单位已有装备去除
                ud.Weapon?.GearDrag.OwnerNum.SetActive(false);
                ud.Weapon?.OnRemove();

                //去除该装备的原单位
                if(Gear.OwnerUnitData!=null) Gear.OnRemove();
                Gear.OnAdd(ud);
            }
            else if (item.gameObject.tag == "ArmorSlot" && Gear.Type == "防具")
            {
                GearOwnerIcon[TeamManager.Ins.CurrMbrIdx].SetAsLastSibling();
                OwnerNum.SetActive(true);
                GearType.SetActive(true);

                //当前的单位
                UnitData ud = TeamManager.Ins.TeamData[TeamManager.Ins.CurrMbrIdx];

                if (ud.Armor == Gear) return;

                //把当目标单位已有装备去除
                ud.Armor?.GearDrag.OwnerNum.SetActive(false);
                ud.Armor?.OnRemove();

                //去除该装备的原单位
                if (Gear.OwnerUnitData != null) Gear.OnRemove();
                Gear.OnAdd(ud);
            }
            else if (item.gameObject.tag == "SupporSlot" && Gear.Type == "辅助")
            {
                GearOwnerIcon[TeamManager.Ins.CurrMbrIdx].SetAsLastSibling();
                OwnerNum.SetActive(true);
                GearType.SetActive(true);

                //当前的单位
                UnitData ud = TeamManager.Ins.TeamData[TeamManager.Ins.CurrMbrIdx];

                if (ud.Support == Gear) return;

                //把当目标单位已有装备去除
                ud.Support?.GearDrag.OwnerNum.SetActive(false);
                ud.Support?.OnRemove();

                //去除该装备的原单位
                if (Gear.OwnerUnitData != null) Gear.OnRemove();
                Gear.OnAdd(ud);
            }
            else
            {
                transform.SetParent(StartParent);
                transform.localPosition = Vector2.zero;
            }
        }
    }

    private List<RaycastResult> GraphicRaycaster(Vector2 pos)
    {
        var mPointerEventData = new PointerEventData(EventSystem);

        mPointerEventData.position = pos;

        List<RaycastResult> results = new List<RaycastResult>();

        _GraphicRaycaster.Raycast(mPointerEventData, results);

        return results;
    }
}
