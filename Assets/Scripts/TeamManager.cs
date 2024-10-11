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
    public List<UnitData> TeamData = new(), EnemyData = new();
    public Button[] MbrBtn = new Button[4];
    public TextMeshProUGUI[] 
        MbrBtnTMP = new TextMeshProUGUI[4], 
        BloodNameTMP = new TextMeshProUGUI[10],
        BloodPointTMP = new TextMeshProUGUI[10],
        TagNodes1 = new TextMeshProUGUI[4], 
        TagNodes2 = new TextMeshProUGUI[4], 
        TagNodes3 = new TextMeshProUGUI[4],
        TagNodes4 = new TextMeshProUGUI[4],
        BloodName,
        BloodPoint;
    public TextMeshProUGUI SkillInfoTMP;
    public Image DetailIcon;
    public Image[] 
        MbrImgs = new Image[4], 
        DetailImgs = new Image[5], 
        //GearSlotImgs = new Image[3],
        Arrows = new Image[4],
        BloodImgs;
    public int CurrMbrIdx;
    public bool TagChanged = true;
    public GameObject TeamNode,DetailNode,TagPrefab,BloodPrefab;
    public List<TextMeshProUGUI[]> TagNodes = new();

    //TODO 火属性额外伤害， 水属性生命上限治疗效果，风属性速度，雷属性魔抗，土属性物抗护盾 在血脉中实现
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
        TeamData.Add(new UnitData(CSVManager.Ins.Units["剑士"],1));
        TeamData.Add(new UnitData(CSVManager.Ins.Units["剑士"], 2));
        TeamData.Add(new UnitData(CSVManager.Ins.Units["火焰法师"], 3));
        TeamData.Add(new UnitData(CSVManager.Ins.Units["火焰法师"], 4));

        TagNodes.Add(TagNodes1);
        TagNodes.Add(TagNodes2);
        TagNodes.Add(TagNodes3);
        TagNodes.Add(TagNodes4);
    }

    //在UI/TeamBtn按钮上绑定 刷新小队面板
    public void ShowTeam()
    {
        TeamNode.SetActive(true);

        foreach (var item in Arrows)
        {
            item.enabled = false;
        }
        Arrows[CurrMbrIdx].enabled = true;

        //设置标签
        for (int i = 0; i < TeamData.Count; i++)
        {
            for (int j = 0; j< 4; j++)
            {
                if(j < TeamData[i].Tags.Length )
                {
                    TagNodes[i][j].text = TeamData[i].Tags[j];
                    TagNodes[i][j].transform.parent.gameObject.SetActive(true);
                }
                else 
                {
                    TagNodes[i][j].transform.parent.gameObject.SetActive(false);
                }
            }
        }

        //属性
        int counter = 0;
        foreach (var item in TeamData[CurrMbrIdx].Bloods)
        {
            if (item.Value == 0) continue;
            BloodNameTMP[counter].transform.parent.parent.gameObject.SetActive(true);
            BloodNameTMP[counter].text = item.Name;
            BloodPointTMP[counter].text = item.Value.ToString();
            BloodImgs[counter].sprite = CSVManager.Ins.BloodIcons[item.Name];
            counter += 1;
        }
        for (int i = counter; i < 10; i++)
        {
            BloodNameTMP[i].transform.parent.parent.gameObject.SetActive(false);
        }

        //队员按钮
        int TCounter = 0;
        for (int i = 0; i < TeamData.Count; i++)
        {
            //MbrImgs[i].sprite = CSVManager.Ins.Character[TeamData[i].Name];
            MbrBtnTMP[i].text = TeamData[i].Name;
            TCounter += 1;
        }
        if (TCounter < 4)
        {
            for (int i = TCounter; i < 4; i++)
            { 
                MbrBtnTMP[i].transform.parent.gameObject.SetActive(false);
            }
        }

        SkillInfoTMP.text = TeamData[CurrMbrIdx].SkillDscrp;

    }
}
[Serializable]
public class UnitData
{
    public int MaxHp, Atk, Cell, Speed;
    public string Name,SkillDscrp;
    public string[] Tags = new string[4];
    public List<Blood> Bloods = new();
    public Sprite sprite;

    public UnitData(string[] data, int cell)
    {
        Name = data[1];
        Cell = cell;

        MaxHp   = int.Parse(data[2]);
        Atk     = int.Parse(data[3]);
        Speed   = int.Parse(data[4]);
        SkillDscrp = data[5];
        Tags    = data[6].Split("&");
        if(data[7]!="") Bloods.Add(new Blood("火元素",float.Parse(data[7])));
        if(data[8]!="") Bloods.Add(new Blood("水元素", float.Parse(data[8])));
        if(data[9]!="") Bloods.Add(new Blood("风元素", float.Parse(data[9])));
        if(data[10]!="") Bloods.Add(new Blood("雷元素", float.Parse(data[10])));
        if(data[11]!="") Bloods.Add(new Blood("土元素", float.Parse(data[11])));
        float yitai = 100;
        foreach (var item in Bloods)
        {
            yitai -= item.Value;
        }
        Bloods.Add(new Blood("以太", yitai));
    }
}
[Serializable]
public class Blood
{
    public string Name;
    public float Value;
    public int Level;
    public Blood(string name, float value)
    {
        Name = name;
        Value = value;
        SetLevel();
    }

    void SetLevel()
    {
        if (EventsMgr.Ins.Level1Blood.Contains(Name)) Level = 1;
        else if (EventsMgr.Ins.Level2Blood.Contains(Name)) Level = 2;
        else if (EventsMgr.Ins.Level3Blood.Contains(Name)) Level = 3;
    }
}
