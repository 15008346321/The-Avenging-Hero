using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BagManager : MonoBehaviour
{
    public int CurrSlot,CurrMbr, Gold, ReptFire;
    public Button[] MbrBtns = new Button[4];
    public Image[] MbrImgs = new Image[4];
    public List<Item> BagLt = new();
    public TextMeshProUGUI DscrpTMP,DscrpNameTMP, UseNameTMP, OwnedTMP, SelTMP;
    public int[] BagPoint = { 0, 0, 0, 0, 0 }, BagPointMax = { 0, 0, 0, 0, 0 };
    public List<int> GotRelics = new();
    public static BagManager Ins;
    public Transform Slots,Use;
    public Image DscrpImg,UseImg;
    public Button DscrpUseBtn,AddBtn, SubBtn, Add10Btn, Sub10Btn, UseBtn;
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    private void Start()
    {
        InitBag();
    }


    public void InitBag()
    {
        SetBagValue();
        InitNode();
        OpenAndRefreshBag();
    }


    public void SetBagValue(string name = "背包")
    {
        //TODO读取存档
    }

    public void InitNode()
    {
        Slots = transform.Find("Bag/slots");
        DscrpImg = transform.Find("Bag/dscrp/Img").GetComponent<Image>();
        DscrpTMP = transform.Find("Bag/dscrp/dscrpTMP").GetComponent<TextMeshProUGUI>();
        DscrpNameTMP = transform.Find("Bag/dscrp/nameTMP").GetComponent<TextMeshProUGUI>();
        DscrpUseBtn = transform.Find("Bag/dscrp/dscrpUseBtn").GetComponent<Button>();
        Use = transform.Find("Bag/use");
        UseImg = Use.Find("bg/Img").GetComponent<Image>();
        UseNameTMP = Use.Find("bg/nameTMP").GetComponent<TextMeshProUGUI>();
        OwnedTMP = Use.Find("bg/ownedTMP").GetComponent<TextMeshProUGUI>();
        SelTMP = Use.Find("bg/selTMP").GetComponent<TextMeshProUGUI>();
        AddBtn = Use.Find("bg/addBtn").GetComponent<Button>();
        SubBtn = Use.Find("bg/subBtn").GetComponent<Button>();
        Add10Btn = Use.Find("bg/add10Btn").GetComponent<Button>();
        Sub10Btn = Use.Find("bg/sub10Btn").GetComponent<Button>();
        UseBtn = Use.Find("bg/useBtn").GetComponent<Button>();
        for (int i = 0; i < Use.Find("bg/mbrBtns").childCount; i++)
        {
            var j = i;
            MbrBtns[i] = Use.Find("bg/mbrBtns").GetChild(i).GetComponent<Button>();
            MbrImgs[i] = Use.Find("bg/mbrBtns").GetChild(i).GetComponent<Image>();
            MbrBtns[i].onClick.AddListener(() => { CurrMbr = j; });
        }
        DscrpUseBtn.onClick.AddListener(UseItem);
        AddBtn.onClick.AddListener(OnAddBtnClick);
        SubBtn.onClick.AddListener(OnSubBtnClick);
        Add10Btn.onClick.AddListener(OnAdd10BtnClick);
        Sub10Btn.onClick.AddListener(OnSub10BtnClick);
    }

    public void AddSkillToBag(string propName, int numericValue)
    {
        BagLt.Add(new Item(propName, numericValue,true));
    }

    public void AddAttrStoneToBag(string propName, int numericValue)
    {
        BagLt.Add(new Item(propName, numericValue,false));
    }
    public void AddGold(int num)
    {
        BagManager.Ins.Gold += num;
        //TODO update ui
    }

    public void AddPoint(string type, int num)
    {
        int idx = 0;
        if (type == "火") idx = 0;
        if (type == "水") idx = 1;
        if (type == "风") idx = 2;
        if (type == "雷") idx = 3;
        if (type == "土") idx = 4;

        BagPoint[idx] += num;
        BagPointMax[idx] += num;
    }

    //绑定在UI/bagbtn上调用
    public void OpenAndRefreshBag()
    {
        foreach (var item in Slots.GetComponentsInChildren<Button>())
        {
            item.enabled = false;
        }
        for (int i = 0; i < Slots.childCount; i++)
        {
            Slots.GetChild(i).GetChild(0).GetComponent<Image>().enabled = false;
        }

        if (BagLt.Count == 0)
        {
            DscrpImg.enabled = false;
            DscrpTMP.text = null;
            DscrpNameTMP.text = null;
            DscrpUseBtn.gameObject.SetActive(false);
        }
        else
        {
            DscrpUseBtn.gameObject.SetActive(true);

            for (int i = 0; i < BagLt.Count; i++)
            {
                Transform node = Slots.GetChild(i);
                node.GetComponent<Button>().enabled = true;

                Image img = node.GetChild(0).GetComponent<Image>();
                img.enabled = true;

                img.sprite = BagLt[i].Img;
                
                Slots.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = BagLt[i].Num.ToString();
            }
            ShowDscrp();
        }
    }

    public void ShowDscrp()
    {
        DscrpNameTMP.text = BagLt[CurrSlot].Name;
        DscrpTMP.text = BagLt[CurrSlot].Dscrp;
        DscrpImg.sprite = BagLt[CurrSlot].Img;
    }

    //背包点击使用调出使用面板
    public void UseItem()
    {
        Use.gameObject.SetActive(true);
        UseNameTMP.text = BagLt[CurrSlot].Name;
        OwnedTMP.text = BagLt[CurrSlot].Num.ToString();
        UseImg.sprite = BagLt[CurrSlot].Img;
        SelTMP.text = "1";

        foreach (var item in MbrBtns)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < TeamManager.Ins.TeamData.Count; i++)
        {
            MbrBtns[i].gameObject.SetActive(true);
            MbrImgs[i].sprite = TeamManager.Ins.TeamData[i].sprite;
        }

    }

    public void OnUseClick()
    {
        int intSel = int.Parse(SelTMP.text);
        if (BagLt[CurrSlot].Name == "力量结晶") TeamManager.Ins.TeamData[CurrMbr].Atk += intSel;
        else if (BagLt[CurrSlot].Name == "火结晶") TeamManager.Ins.TeamData[CurrMbr].Fire += intSel;
        else if (BagLt[CurrSlot].Name == "水结晶") TeamManager.Ins.TeamData[CurrMbr].Water += intSel;
        else if (BagLt[CurrSlot].Name == "风结晶") TeamManager.Ins.TeamData[CurrMbr].Wind += intSel;
        else if (BagLt[CurrSlot].Name == "雷结晶") TeamManager.Ins.TeamData[CurrMbr].Thunder += intSel;
        else if (BagLt[CurrSlot].Name == "土结晶") TeamManager.Ins.TeamData[CurrMbr].Earth += intSel;
        else
        {
            //TODO使用技能书
        }
        BagLt[CurrSlot].Num -= intSel;
        //TODO做一个属性提升动画
    }

    public void OnAddBtnClick()
    {
        var curr = int.Parse(SelTMP.text) + 1;
        if (curr < BagLt[CurrSlot].Num) SelTMP.text = curr.ToString();
        else SelTMP.text = BagLt[CurrSlot].Num.ToString();
    }

    public void OnSubBtnClick()
    {
        var curr = int.Parse(SelTMP.text) - 1;
        if (curr > 1) SelTMP.text = curr.ToString();
        else SelTMP.text = "1";
    }

    public void OnAdd10BtnClick()
    {
        var curr = int.Parse(SelTMP.text) + 10;
        if (curr < BagLt[CurrSlot].Num) SelTMP.text = curr.ToString();
        else SelTMP.text = BagLt[CurrSlot].Num.ToString();
    }

    public void OnSub10BtnClick()
    {
        var curr = int.Parse(SelTMP.text) - 10;
        if (curr > 1) SelTMP.text = curr.ToString();
        else SelTMP.text = "1";
    }
}

[System.Serializable]
public class Item
{
    public string Name{ get;}
    public int Num{ get; set; }
    public Sprite Img{ get;}
    public string Dscrp { get; }
    public Item(string name, int num, bool isBook)
    {
        Name = name;
        Num = num;
        if (isBook)
        {
            string type = CSVManager.Ins.全技能表[name][2];
            if (type == "普攻") Img = Resources.Load<Sprite>("Texture/Icon/Items/book1");
            if (type == "追打") Img = Resources.Load<Sprite>("Texture/Icon/Items/book2");
            if (type == "被动") Img = Resources.Load<Sprite>("Texture/Icon/Items/book3");
            Dscrp = CSVManager.Ins.全技能表[name][5];
        }
        else
        {
            Name = name + "结晶";
            Img = Resources.Load<Sprite>("Texture/Icon/Items/" + Name);
            Dscrp = CSVManager.Ins.全物品表[Name][2];
        }
    }
}