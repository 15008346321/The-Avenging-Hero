using DG.Tweening;
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
        DetailPassiveTMP = new TextMeshProUGUI[4];
    public TextMeshProUGUI DetailName,AtkName, AtkDscrp, CombName, CombDscrp, WeaponName, WeaponDscrp, 
        ArmorName, ArmorDscrp, SupportName, SupportDscrp;
    public Image DetailIcon, WeaponSlotImg;
    public Image[] MbrImgs = new Image[4], DetailImgs = new Image[5], GearSlotImgs = new Image[3];
    public int CurrMbrIdx = 0;
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

        //名字头像
        DetailName.text = TeamData[CurrMbrIdx].Name;
        //DetailIcon.sprite = CSVManager.Ins.Character[DetailName.text];

        //属性
        DetailAttrTMP[0].text = TeamData[CurrMbrIdx].MaxHp.ToString();
        DetailAttrTMP[1].text = TeamData[CurrMbrIdx].Speed.ToString();
        DetailAttrTMP[2].text = TeamData[CurrMbrIdx].Atk.ToString();
        DetailAttrTMP[3].text = TeamData[CurrMbrIdx].Fire.ToString();
        DetailAttrTMP[4].text = TeamData[CurrMbrIdx].Water.ToString();
        DetailAttrTMP[5].text = TeamData[CurrMbrIdx].Wind.ToString();
        DetailAttrTMP[6].text = TeamData[CurrMbrIdx].Thunder.ToString();
        DetailAttrTMP[7].text = TeamData[CurrMbrIdx].Earth.ToString();

        //技能装备
        AtkName.text = TeamData[CurrMbrIdx].AtkName;
        AtkDscrp.text = CSVManager.Ins.Atks[AtkName.text][2];

        CombName.text = TeamData[CurrMbrIdx].CombName;
        CombDscrp.text = CSVManager.Ins.Combs[CombName.text][2];

        WeaponName.text = TeamData[CurrMbrIdx].Weapon?.Name;
        WeaponDscrp.text = TeamData[CurrMbrIdx].Weapon?.Dscrp;

        ArmorName.text = TeamData[CurrMbrIdx].Armor?.Name;
        ArmorDscrp.text = TeamData[CurrMbrIdx].Armor?.Dscrp;

        SupportName.text = TeamData[CurrMbrIdx].Support?.Name;
        SupportDscrp.text = TeamData[CurrMbrIdx].Support?.Dscrp;

        //DetailImgs[0].sprite = CSVManager.Ins.Items[AtkName.text];
        //DetailImgs[1].sprite = CSVManager.Ins.Items[CombName.text];
        //DetailImgs[2].sprite = CSVManager.Ins.Items[WeaponName.text];
        //DetailImgs[3].sprite = CSVManager.Ins.Items[ArmorName.text];
        //DetailImgs[4].sprite = CSVManager.Ins.Items[SupportName.text];

        //被动
        DetailPassiveTMP[0].text = TeamData[CurrMbrIdx].Passive1;
        DetailPassiveTMP[1].text = TeamData[CurrMbrIdx].Passive2;
        DetailPassiveTMP[2].text = TeamData[CurrMbrIdx].Passive3;
        DetailPassiveTMP[3].text = TeamData[CurrMbrIdx].Passive4;

        //队员按钮
        for (int i = 0; i < TeamData.Count -1; i++)
        {
            //MbrImgs[0].sprite = CSVManager.Ins.Character[TeamData[i].Name];
            MbrBtnTMP[i].text = TeamData[i].Name;
            MbrImgs[i].sprite = TeamData[i].sprite;
        }
        ShowDetail();
    }

    public void ShowGearSlot(int i)
    {
        GearSlotImgs[i].DOFade(0.75f,1f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }

    public void StopShowGearSlot()
    {
        foreach (var item in GearSlotImgs)
        {
            item.DOKill();
            WeaponSlotImg.DOFade(0.5f, 0f);
        }
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
    public string Name,AtkName,CombName,WeaponName,ArmorName,SupportName,Passive1, Passive2, Passive3, Passive4;
    public Sprite sprite;
    public ComponentBase NormalAtk, Comb, Weapon, Armor, Support;

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
        Passive1  = data[12];

        //Todo 特性 装备
        //sprite = CSVManager.Ins.Character[Name];
    }
}
