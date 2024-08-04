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
    public Button[] MbrBtn = new Button[4];
    public TextMeshProUGUI[] MbrBtnTMP = new TextMeshProUGUI[4], DetailAttrTMP = new TextMeshProUGUI[8],
        DetailTraitTMP = new TextMeshProUGUI[4];
    public TextMeshProUGUI DetailName,AtkName, AtkDscrp, CombName, CombDscrp, WeaponName, WeaponDscrp, 
        ArmorName, ArmorDscrp, SupportName, SupportDscrp;
    public Image DetailIcon;
    public Image[] MbrImgs = new Image[4], DetailImgs = new Image[5];
    public int MbrInfoIdx = 0;
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
    }
    private void InitTeam()
    {
        //TODO改到配置表中
        TeamData.Add(new UnitData(CSVManager.Ins.Units["初级剑士"],1));
        TeamData.Add(new UnitData(CSVManager.Ins.Units["初级剑士"],2));
        TeamData.Add(new UnitData(CSVManager.Ins.Units["初级剑士"],3));
        TeamData.Add(new UnitData(CSVManager.Ins.Units["初级剑士"],4));
    }

    //在UI/TeamBtn按钮上绑定
    public void ShowTeam()
    {
        TeamNode.SetActive(true);

        DetailName.text = TeamData[MbrInfoIdx].Name;
        //DetailIcon.sprite = CSVManager.Ins.Character[DetailName.text];

        DetailAttrTMP[0].text = TeamData[MbrInfoIdx].MaxHp.ToString();
        DetailAttrTMP[1].text = TeamData[MbrInfoIdx].Speed.ToString();
        DetailAttrTMP[2].text = TeamData[MbrInfoIdx].Atk.ToString();
        DetailAttrTMP[3].text = TeamData[MbrInfoIdx].Fire.ToString();
        DetailAttrTMP[4].text = TeamData[MbrInfoIdx].Water.ToString();
        DetailAttrTMP[5].text = TeamData[MbrInfoIdx].Wind.ToString();
        DetailAttrTMP[6].text = TeamData[MbrInfoIdx].Thunder.ToString();
        DetailAttrTMP[7].text = TeamData[MbrInfoIdx].Earth.ToString();

        AtkName.text = TeamData[MbrInfoIdx].AtkName;
        AtkDscrp.text = CSVManager.Ins.Atks[AtkName.text][2];

        CombName.text = TeamData[MbrInfoIdx].CombName;
        CombDscrp.text = CSVManager.Ins.Combs[CombName.text][2];

        WeaponName.text = TeamData[MbrInfoIdx].WeaponName;
        WeaponDscrp.text = CSVManager.Ins.Weapons[WeaponName.text][2];

        ArmorName.text = TeamData[MbrInfoIdx].ArmorName;
        ArmorDscrp.text = CSVManager.Ins.Armors[ArmorName.text][2];

        SupportName.text = TeamData[MbrInfoIdx].SupportName;
        SupportDscrp.text = CSVManager.Ins.Supports[SupportName.text][2];

        //DetailImgs[0].sprite = CSVManager.Ins.Items[AtkName.text];
        //DetailImgs[1].sprite = CSVManager.Ins.Items[CombName.text];
        //DetailImgs[2].sprite = CSVManager.Ins.Items[WeaponName.text];
        //DetailImgs[3].sprite = CSVManager.Ins.Items[ArmorName.text];
        //DetailImgs[4].sprite = CSVManager.Ins.Items[SupportName.text];

        for (int i = 0; i < TeamData.Count -1; i++)
        {
            //MbrImgs[0].sprite = CSVManager.Ins.Character[TeamData[i].Name];
            MbrBtnTMP[i].text = TeamData[i].Name;
            MbrImgs[i].sprite = TeamData[i].sprite;
        }
        ShowDetail();
    }
    //Btn上绑定调用
    public void ShowDetail()
    {
        DetailNode.SetActive(true);
        //AtkName.text = TeamData[MbrInfoIdx].NormalAtk.Name;
        //AtkDscrp.text = TeamData[MbrInfoIdx].NormalAtk.Dscrp;
        //CombName.text = TeamData[MbrInfoIdx].Comb.Name;
        //CombDscrp.text = TeamData[MbrInfoIdx].Comb.Dscrp;
        //SpecialName1.text = TeamData[MbrInfoIdx].Special.Name;
        //SpecialDscrp1.text = TeamData[MbrInfoIdx].Special.Dscrp;
        //WeaponName.text = TeamData[MbrInfoIdx].relic1.Name;
        //WeaponDscrp.text = TeamData[MbrInfoIdx].relic1.Dscrp;
        //ArmorName.text = TeamData[MbrInfoIdx].relic2.Name;
        //ArmorDscrp.text = TeamData[MbrInfoIdx].relic2.Dscrp;
        //SupportName.text = TeamData[MbrInfoIdx].relic3.Name;
        //SupportDscrp.text = TeamData[MbrInfoIdx].relic3.Dscrp;
    }
}
[Serializable]
public class UnitData
{
    public int MaxHp, Atk, Fire, Water, Wind, Thunder, Earth, Cell,Speed ;
    public string Name,AtkName,CombName,WeaponName,ArmorName,SupportName,Unique1, Unique2, Unique3, Unique4;
    public Sprite sprite;

    public UnitData(string[] data, int pos)
    {
        Name = data[1];
        Cell = pos;

        int result;
        MaxHp   = int.Parse(data[2]);
        Atk     = int.TryParse(data[3], out result) ? result : 0;
        Fire    = int.TryParse(data[4], out result) ? result : 0;
        Water   = int.TryParse(data[5], out result) ? result : 0;
        Wind    = int.TryParse(data[6], out result) ? result : 0;
        Thunder = int.TryParse(data[7], out result) ? result : 0;
        Earth   = int.TryParse(data[8], out result) ? result : 0;
        Speed   = int.Parse(data[9]);
        AtkName  = data[10];
        CombName = data[11];
        Unique1  = data[12];

        //Todo 特性 装备
        //sprite = CSVManager.Ins.Character[Name];
    }
}
