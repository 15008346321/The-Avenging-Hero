using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVMgr : MonoBehaviour
{
    public static CSVMgr Ins;
    public List<string[]> StartText, Events1;
    public Dictionary<string, string[]> Units, Atks,Combs,Uniques,Gears;
    public Dictionary<string, Sprite> 角色图片 = new(),遗物图片 = new(),TypeIcon = new(),BgSprites = new();
    public string[] 角色名称数组;

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);

        Init();
    }

    public void Init()
    {
        //StartText = GetStartText();
        Units = GetUnit();
        角色名称数组 = Units.Keys.ToArray();

        GetTypeIcon();
        GetBgSprites();
        初始化角色图片();
        初始化遗物图片();
    }

    private void GetTypeIcon()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Icon/Types");

        foreach (var item in sprites)
        {
            TypeIcon.Add(item.name, item);
        }
    }

    private void GetBgSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/BG");

        foreach (var item in sprites)
        {
            BgSprites.Add(item.name, item);
        }
    }

    private void 初始化角色图片()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Icon/Character");

        foreach (var item in sprites)
        {
            角色图片.Add(item.name, item);
        }
    }

    private void 初始化遗物图片()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/遗物");

        foreach (var item in sprites)
        {
            遗物图片.Add(item.name, item);
        }
    }

    public List<string[]> ReadCSVToList(string pathType, string fileName)
    {

        string filePath = "Configs/" + pathType + "/" + fileName;

        TextAsset ta = Resources.Load<TextAsset>(filePath);

        string csv = ta.text;

        StringReader reader = new StringReader(csv);
        //去掉第一行
        reader.ReadLine();

        string line;

        List<string[]> data = new();

        while ((line = reader.ReadLine()) != null)
        {
            string[] values = line.Split(',');

            data.Add(values);
        }
        return data;
    }

    public Dictionary<string,string[]> ReadCSVToDict(string pathType,string fileName)
    {
        string filePath = "Configs/" + pathType + "/" + fileName;

        TextAsset ta = Resources.Load<TextAsset>(filePath);

        string csv = ta.text;

        StringReader reader = new StringReader(csv);
        //去掉第一行
        reader.ReadLine();

        string line;

        Dictionary<string, string[]> data = new();

        while ((line = reader.ReadLine()) != null)
        {
            string[] values = line.Split(',');

            data.Add(values[1],values);
        }
        return data;
    }

    public Dictionary<string, string[]> GetUnit()
    {
        return ReadCSVToDict("Units", "Units");
    }

    public List<string[]> GetEventsByArea(string Area = "area1")
    {
        return ReadCSVToList("Events", Area);
    }

    public List<string[]> GetBattlesByArea(string Area = "area1")
    {
        return ReadCSVToList("Battles", Area);
    }

    public void SaveTeamToCSV()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Team.csv");
        using StreamWriter writer = new StreamWriter(filePath);
        foreach (var unit in TeamMgr.Ins.TeamData)
        {
            //writer.WriteLine($"{unit.Name},{unit.MaxHp},{unit.Atk},{unit.Fire},{unit.Water}" +
            //    $",{unit.Wind},{unit.Thunder},{unit.Earth},{unit.Special},{unit.NormalAtk},{unit.Comb}" +
            //    $",{unit.Special},{unit.Cell}");
        }
    }
}
