using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamMgr : MonoBehaviour
{
    public static TeamMgr Ins;
    public List<UnitData> TeamData = new(),拥有角色数据 = new(), EnemyData = new();
    public List<角色栏拖拽> 角色栏列表;
    public Button[] MbrBtn = new Button[4];
    public TextMeshProUGUI[]
        uGUIs = new TextMeshProUGUI[5],
        MbrBtnTMP = new TextMeshProUGUI[4], 
        BloodNameTMP = new TextMeshProUGUI[14],
        BloodPointTMP = new TextMeshProUGUI[14],
        TagNodes1 = new TextMeshProUGUI[4], 
        TagNodes2 = new TextMeshProUGUI[4], 
        TagNodes3 = new TextMeshProUGUI[4],
        TagNodes4 = new TextMeshProUGUI[4],
        BloodName,
        BloodPoint;
    public TextMeshProUGUI SkillInfoTMP;
    public Image[]
        MbrImgs = new Image[4],
        DetailImgs = new Image[5],
        //GearSlotImgs = new Image[3],
        Arrows = new Image[4],
        BloodImgs = new Image[14];
    public int CurrMbrIdx,BloodInsNum = 14;
    public Transform BloodParent,角色槽位;
    public GameObject TeamNode,DetailNode,BloodPrefab;

    //TODO 火属性额外伤害， 水属性生命上限治疗效果，风属性速度，雷属性魔抗，土属性物抗护盾 在血脉中实现
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }
    void Start()
    {
        //Init();
    }
    private void Init()
    {
        //TODO改到配置表中
        TeamData.Add(new UnitData(CSVMgr.Ins.Units["火焰剑士"]));
        TeamData.Add(new UnitData(CSVMgr.Ins.Units["火焰法师"], 2));
        TeamData.Add(new UnitData(CSVMgr.Ins.Units["猫妖"], 3));
        TeamData.Add(new UnitData(CSVMgr.Ins.Units["嗜血战士"], 4));
        for (int i = 0; i < TeamData.Count; i++)
        {
            拥有角色数据.Add(TeamData[i]);
        }
        拥有角色数据.Add(new UnitData(CSVMgr.Ins.Units["骑士"]));
        //TeamData.Add(new UnitData(CSVManager.Ins.Units["猫妖"], 3));
        //TeamData.Add(new UnitData(CSVManager.Ins.Units["嗜血战士"], 4));

        for (int i = 0; i < BloodInsNum; i++)
        {
            GameObject g = Instantiate(BloodPrefab);
            g.transform.SetParent(BloodParent);
            TextMeshProUGUI[] uGUIs = g.transform.GetComponentsInChildren<TextMeshProUGUI>();
            BloodNameTMP[i] = uGUIs[0];
            BloodPointTMP[i] = uGUIs[1];
            BloodImgs[i] = g.transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void 生成随机初始角色()
    {
        var idx = UnityEngine.Random.Range(0, CSVMgr.Ins.角色名称数组.Length);
        var name = CSVMgr.Ins.角色名称数组[idx];
        TeamData.Add(new UnitData(CSVMgr.Ins.Units[name]));
        拥有角色数据.Add(TeamData[0]);
    }

    public void 更新角色栏数据() 
    {
        for (int i = 0; i < 角色栏列表.Count; i++)
        {
            角色栏列表[i].角色栏图片.enabled = false;
        }

        for (int i = 0; i < 拥有角色数据.Count; i++)
        {
            角色栏列表[i].unitData = 拥有角色数据[i];
            角色栏列表[i].角色栏图片.enabled = true;
            角色栏列表[i].角色栏图片.sprite = 拥有角色数据[i].角色图片;
        }
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

        //属性
        int counter = 0;
        foreach (var item in TeamData[CurrMbrIdx].Bloods)
        {
            if (item.Value == 0) continue;
            BloodNameTMP[counter].transform.parent.parent.gameObject.SetActive(true);
            BloodNameTMP[counter].text = item.Name.ToString();
            BloodPointTMP[counter].text = item.Value.ToString();
            BloodImgs[counter].sprite = CSVMgr.Ins.TypeIcon[item.Name.ToString()];
            counter += 1;
        }
        for (int i = counter; i < BloodInsNum; i++)
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

        StringBuilder sb = new();
        foreach (var item in TeamData[CurrMbrIdx].SkillDscrp)
        {
            sb.AppendLine(item);
        }
        SkillInfoTMP.text = sb.ToString();

    }

}
[Serializable]
public class UnitData
{
    public int MaxHp, Atk, Cell, Speed, SkillPointMax, 在神像位置 = -1;
    public string Name, 技能描述_TMP, 血脉_TMP, original = "<sprite=\"血脉\" name=\"{0}\">{1} {2} ";
    public string[] SkillDscrp;
    public List<Blood> Bloods = new();
    public Sprite 角色图片;
    public 角色评级Enum 角色评分
    { 
        get
        {
            float 血脉总和 = 2* Bloods.Sum(u => u.Value);
            float 属性总分 = 血脉总和 + MaxHp + 2 * Speed + 3 * Atk;
            if (属性总分 <= 50)
            {
                return 角色评级Enum.F;
            }
            else if(属性总分 <= 100)
            {
                return 角色评级Enum.E;
            }
            else if (属性总分 <= 200)
            {
                return 角色评级Enum.D;
            }
            else if (属性总分 <= 300)
            {
                return 角色评级Enum.C;
            }
            else if (属性总分 <= 500)
            {
                return 角色评级Enum.B;
            }
            else if (属性总分 <= 700)
            {
                return 角色评级Enum.A;
            }
            else if (属性总分 <= 1000)
            {
                return 角色评级Enum.S;
            }
            else
            {
                return 角色评级Enum.SSS;
            }
        }
    }

    public int 角色价格
    {
        get
        {
            switch (角色评分)
            {
                case 角色评级Enum.F:
                    return 5;
                case 角色评级Enum.E:
                    return 10;
                case 角色评级Enum.D:
                    return 20;
                case 角色评级Enum.C:
                    return 30;
                case 角色评级Enum.B:
                    return 50;
                case 角色评级Enum.A:
                    return 70;
                case 角色评级Enum.S:
                    return 100;
                case 角色评级Enum.SSS:
                    return 150;
                default:
                    return 0;
            }
        }
    }

    public UnitData(string[] data, int cell = 1)
    {
        Name = data[1];
        Cell = cell;

        MaxHp   = int.Parse(data[2]);
        Atk     = int.Parse(data[3]);
        Speed   = int.Parse(data[4]);
        
        SkillDscrp = data[5].Split("\n");

        角色图片 = CSVMgr.Ins.角色图片[Name];


        解析血脉(data[6]);
        
        int.TryParse(data[7], out SkillPointMax);
    }

    void 解析血脉(string 血脉数据) 
    {
        string[] data = 血脉数据.Split(";").Where(s => !string.IsNullOrEmpty(s)).ToArray();

        foreach (var item in data)
        {
            string[] per = item.Split(":");
            Bloods.Add(new Blood((魔力类型Enum)Enum.Parse(typeof(魔力类型Enum),per[0]), float.Parse(per[1])));
            血脉_TMP += string.Format(original, per[0], per[0], per[1]);
        }
    }
}
[Serializable]
public class Blood
{
    public 魔力类型Enum Name;
    public float Value;
    public int Level;
    public Blood(魔力类型Enum _魔力类型, float value)
    {
        Name = _魔力类型;
        Value = value;
        SetLevel();
    }

    void SetLevel()
    {
        if (EventsMgr.Ins.Level0Blood.Contains(Name)) Level = 0;
        else if (EventsMgr.Ins.Level1Blood.Contains(Name)) Level = 1;
        else if (EventsMgr.Ins.Level2Blood.Contains(Name)) Level = 2;
        else if (EventsMgr.Ins.Level3Blood.Contains(Name)) Level = 3;
    }
}

