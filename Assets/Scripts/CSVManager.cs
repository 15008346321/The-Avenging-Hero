using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class CSVManager : MonoBehaviour
{
    public static CSVManager Ins;
    public List<string[]> StartText, Events1;
    public Dictionary<string, string[]> Units, Atks,Combs,Uniques,Weapons, Armors, Supports;
    public Dictionary<string, Sprite> Character;

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);

        Init();
    }

    public void Init()
    {
        StartText = GetStartText();
        Units = GetUnit();
        Atks = GetAtks();
        Combs = GetCombs();
        //Skills = GetSkills();
        //Features = GetFeatures();
        //Relics = GetRelics();
        //Goods = GetGoods();
        //Cities = GetCities();
        //Character = GetCharacterSprites();
        //0id 1名称 2属性 3最大生命值 4攻击 5防御 6法术 7魔抗
        //8火 9水 10风 11雷 12土 13冰 14光 15暗 16普攻 17追打 18被动
        //0id 1价格 2名称 3类型 4效果 5code 6普攻总数,7追打总数
        //8造成状态,9追打状态,10被动总数,11充能总数
        //foreach (var item in Features)
        //{
        //    全特性表.Add(item[1], item);
        //}
        //foreach (var item in Relics)
        //{
        //    全遗物表.Add(item[1], item);
        //}
        //foreach (var item in Goods)
        //{
        //    全物品表.Add(item[1], item);
        //}
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
    public List<string[]> ReadCSVToList(string pathType, string fileName)
    {
        string filePath = Application.dataPath + "/Configs/" + pathType + "/" + fileName + ".csv";

        using StreamReader sr = new(filePath, Encoding.UTF8);

        List<string[]> rowData = new();
        if (!sr.EndOfStream)
        {
            sr.ReadLine();
        }
        while (!sr.EndOfStream)
        {
            string[] row = sr.ReadLine().Replace("\"", "").Split(',');
            rowData.Add(row);
        }
        return rowData;
    }

    public Dictionary<string,string[]> ReadCSVToDict(string pathType,string fileName)
    {
        string filePath = Application.dataPath + "/Configs/" + pathType + "/"+ fileName + ".csv";

        using StreamReader sr = new(filePath, Encoding.UTF8);

        Dictionary<string, string[]> rowData = new();
        if (!sr.EndOfStream)
        {
            sr.ReadLine();
        }
        while (!sr.EndOfStream)
        {
            string[] row = sr.ReadLine().Replace("\"","").Split(',');
            rowData.Add(row[1],row);
        }

        return rowData;
    }

    public List<string[]> GetStartText()
    {
        return ReadCSVToList("StartText", "StartText");
    }

    public Dictionary<string, string[]> GetUnit()
    {
        return ReadCSVToDict("Units", "Units");
    }

    public Dictionary<string,string[]> GetAtks()
    {
        return ReadCSVToDict("Skills", "Atks");
    }

    public Dictionary<string, string[]> GetCombs()
    {
        return ReadCSVToDict("Skills", "Combs");
    }

    public List<string[]> GetEventsByArea(string Area = "area1")
    {
        return ReadCSVToList("Events", Area);
    }
    public List<string[]> GetBattlesByArea(string Area = "area1")
    {
        return ReadCSVToList("Battles", Area);
    }

    //public List<string[]> GetGoods()
    //{
    //    return ReadCSV("Shop" , "Goods");
    //}
    //public List<string[]> GetRelics()
    //{
    //    return ReadCSV("Relics", "Relics");
    //}
    //public List<string[]> GetSkills()
    //{
    //    return ReadCSV("Skills", "Skills");
    //}

    //public List<string[]> GetFeatures()
    //{
    //    return ReadCSV("Skills", "Features");
    //}

    //private List<string[]> GetCities()
    //{
    //    return ReadCSV("Map", "Cities");
    //}
    public void SaveTeamToCSV()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Team.csv");
        using StreamWriter writer = new StreamWriter(filePath);
        foreach (var unit in TeamManager.Ins.TeamData)
        {
            //writer.WriteLine($"{unit.Name},{unit.MaxHp},{unit.Atk},{unit.Fire},{unit.Water}" +
            //    $",{unit.Wind},{unit.Thunder},{unit.Earth},{unit.Special},{unit.NormalAtk},{unit.Comb}" +
            //    $",{unit.Special},{unit.Cell}");
        }
    }
}
