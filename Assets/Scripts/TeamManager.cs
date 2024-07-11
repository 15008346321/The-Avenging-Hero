using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Ins;
    public List<UnitData> TeamData = new();
    public Button[] MbrInfoBtn = new Button[4], MbrSelBtns = new Button[4];
    public TextMeshProUGUI[] T1AttrTMP = new TextMeshProUGUI[7], T2AttrTMP = new TextMeshProUGUI[7],
        T3AttrTMP = new TextMeshProUGUI[7], T4AttrTMP = new TextMeshProUGUI[7], T1ItemNameTMP = new TextMeshProUGUI[6],
        T2ItemNameTMP = new TextMeshProUGUI[6], T3ItemNameTMP = new TextMeshProUGUI[6], T4ItemNameTMP = new TextMeshProUGUI[6],
        NamesTMP = new TextMeshProUGUI[4];
    public List<TextMeshProUGUI[]> Attrs = new(),Items = new();
    public TextMeshProUGUI AtkName, AtkDscrp, CombName, CombDscrp, SpecialName, SpecialDscrp,
        WeaponName, WeaponDscrp, ArmorName, ArmorDscrp, SupportName, SupportDscrp;
    public Image[] MbrImgs = new Image[4], T1EqpImgs = new Image[6], T2EqpImgs = new Image[6],
        T3EqpImgs = new Image[6], T4EqpImgs = new Image[6],SkillInfoImgs = new Image[6];
    public int MbrSelIdx = 0, MbrInfoIdx = 0;
    public GameObject TeamNode,DetailNode;
    //TODO 火属性额外伤害， 水属性生命上限治疗效果，风属性速度，雷属性魔抗，土属性物抗护盾
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }
    void Start()
    {
        InitTeam();
        Init();
    }
    public void Init()
    {
        Attrs.Add(T1AttrTMP);
        Attrs.Add(T2AttrTMP);
        Attrs.Add(T3AttrTMP);
        Attrs.Add(T4AttrTMP);
        Items.Add(T1ItemNameTMP);
        Items.Add(T2ItemNameTMP);
        Items.Add(T3ItemNameTMP);
        Items.Add(T4ItemNameTMP);
    }
    private void InitTeam()
    {
        //TODO改到配置表中
        TeamData.Add(new UnitData(CSVManager.Ins.模板参数["初级剑士"],1));
        TeamData.Add(new UnitData(CSVManager.Ins.模板参数["初级剑士"],2));
        TeamData.Add(new UnitData(CSVManager.Ins.模板参数["初级剑士"],3));
        TeamData.Add(new UnitData(CSVManager.Ins.模板参数["初级剑士"],4));
    }

    //UI:TeamBtn
    public void ShowTeam()
    {
        TeamNode.SetActive(true);
        int idx = 0;
        foreach (var item in TeamData)
        {
            //NamesTMP[idx].text = item.Name;
            Items[idx][0].text = item.NormalAtk.Name;
            Items[idx][1].text = item.Comb.Name;
            Items[idx][2].text = item.Special.Name;
            Items[idx][3].text = item.relic1.Name;
            Items[idx][4].text = item.relic2.Name;
            Items[idx][5].text = item.relic3.Name;
            //    Attrs[idx][0].text = item.MaxHp.ToString();
            //    Attrs[idx][1].text = item.Atk.ToString();
            //    Attrs[idx][2].text = item.Fire.ToString();
            //    Attrs[idx][3].text = item.Water.ToString();
            //    Attrs[idx][4].text = item.Wind.ToString();
            //    Attrs[idx][5].text = item.Thunder.ToString();
            //    Attrs[idx][6].text = item.Earth.ToString();
            //    idx += 1;
        }
    }
    public void ShowDetail()
    {
        DetailNode.SetActive(true);
        AtkName.text = TeamData[MbrInfoIdx].NormalAtk.Name;
        AtkDscrp.text = TeamData[MbrInfoIdx].NormalAtk.Dscrp;
        CombName.text = TeamData[MbrInfoIdx].Comb.Name;
        CombDscrp.text = TeamData[MbrInfoIdx].Comb.Dscrp;
        SpecialName.text = TeamData[MbrInfoIdx].Special.Name;
        SpecialDscrp.text = TeamData[MbrInfoIdx].Special.Dscrp;
        WeaponName.text = TeamData[MbrInfoIdx].relic1.Name;
        WeaponDscrp.text = TeamData[MbrInfoIdx].relic1.Dscrp;
        ArmorName.text = TeamData[MbrInfoIdx].relic2.Name;
        ArmorDscrp.text = TeamData[MbrInfoIdx].relic2.Dscrp;
        SupportName.text = TeamData[MbrInfoIdx].relic3.Name;
        SupportDscrp.text = TeamData[MbrInfoIdx].relic3.Dscrp;
    }
}
[Serializable]
public class UnitData
{
    public int MaxHp, Atk, Fire, Water, Wind, Thunder, Earth, Cell;
    public string Name;
    public Skill Special, NormalAtk, Comb;
    public Relic relic1, relic2, relic3;
    public Sprite sprite;

    public UnitData(string[] data, int pos)
    {
        Name = data[1];
        Cell = pos;

        MaxHp = int.TryParse(data[2], out int result) ? result : 0;
        Atk     = int.TryParse(data[3], out result) ? result : 0;
        Fire    = int.TryParse(data[4], out result) ? result : 0;
        Water   = int.TryParse(data[5], out result) ? result : 0;
        Wind    = int.TryParse(data[6], out result) ? result : 0;
        Thunder = int.TryParse(data[7], out result) ? result : 0;
        Earth   = int.TryParse(data[8], out result) ? result : 0;

        //sprite = CSVManager.Ins.Character[Name];
    }
}
