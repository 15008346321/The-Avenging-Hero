using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class CSVManager : MonoBehaviour
{
    public static CSVManager Ins;
    public List<string[]> StartText, Units, Skills, Features, Relics, Goods, Cities,
        Events1;
    public Dictionary<string, string[]> 模板参数 = new(), 全技能表 = new(),
        全特性表 = new(), 全遗物表 = new(), 全物品表 = new();
    public Dictionary<string, Sprite> Character = new();

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);

        Init();
    }

    public void Init()
    {
        StartText = GetStartText();
        Units     = GetUnit();
        Skills    = GetSkills();
        Features  = GetFeatures();
        Relics    = GetRelics();
        Goods     = GetGoods();
        Cities    = GetCities();
        Character = GetCharacterSprites();
        //0id 1名称 2属性 3最大生命值 4攻击 5防御 6法术 7魔抗
        //8火 9水 10风 11雷 12土 13冰 14光 15暗 16普攻 17追打 18被动
        foreach (var item in Units)
        {
            模板参数.Add(item[1], item);
        }
        //0id 1价格 2名称 3类型 4效果 5code 6普攻总数,7追打总数
        //8造成状态,9追打状态,10被动总数,11充能总数
        foreach (var item in Features)
        {
            全特性表.Add(item[1], item);
        }
        foreach (var item in Skills)
        {
            全技能表.Add(item[1], item);
        }
        foreach (var item in Relics)
        {
            全遗物表.Add(item[1], item);
        }
        foreach (var item in Goods)
        {
            全物品表.Add(item[1], item);
        }
    }

    private Dictionary<string, Sprite> GetCharacterSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Icon/Character");

        foreach (var item in sprites)
        {
            Character.Add(item.name, item);
        }
        return Character;
    }

    public List<string[]> ReadCSV(string pathType,string fileName)
    {
        string filePath = Application.dataPath + "/Configs/" + pathType + "/"+ fileName + ".csv";

        using StreamReader sr = new(filePath, Encoding.UTF8);

        List<string[]> rowData = new();

        while (!sr.EndOfStream)
        {
            string[] row = sr.ReadLine().Replace("\"","").Split(',');
            rowData.Add(row);
        }

        rowData.RemoveAt(0);

        return rowData;
    }
    public List<string[]> GetStartText()
    {
        return ReadCSV("StartText", "StartText");
    }

    public List<string[]> GetEventsByArea(string Area = "area1")
    {
        return ReadCSV("Events", Area);
    }
    public List<string[]> GetBattlesByArea(string Area = "area1")
    {
        return ReadCSV("Battles", Area);
    }
    public List<string[]> GetUnit()
    {
        return ReadCSV("Units", "Units");
    }
    public List<string[]> GetGoods()
    {
        return ReadCSV("Shop" , "Goods");
    }
    public List<string[]> GetRelics()
    {
        return ReadCSV("Relics", "Relics");
    }
    public List<string[]> GetSkills()
    {
        return ReadCSV("Skills", "Skills");
    }

    public List<string[]> GetFeatures()
    {
        return ReadCSV("Skills", "Features");
    }
    
    private List<string[]> GetCities()
    {
        return ReadCSV("Map", "Cities");
    }
    public void SaveTeamToCSV()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Team.csv");
        using StreamWriter writer = new StreamWriter(filePath);
        foreach (var unit in TeamManager.Ins.TeamData)
        {
            writer.WriteLine($"{unit.Name},{unit.MaxHp},{unit.Atk},{unit.Fire},{unit.Water}" +
                $",{unit.Wind},{unit.Thunder},{unit.Earth},{unit.Special},{unit.NormalAtk},{unit.Comb}" +
                $",{unit.Special},{unit.Cell}");
        }
    }
}
