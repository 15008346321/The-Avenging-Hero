using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public Button StartBtn;
    bool MapGenerated;
    Transform Cities, Description, Pos, MapRoads;
    public GameObject Map;
    public TextMeshProUGUI CityNmaeTMP, CityDesctiptionTMP, DegreeTMP;
    public Dictionary<string, string> CityDescription = new Dictionary<string, string>();
    public Level CurrentLevel;
    public Image[] ChooseBg = new Image[9];
    public List<int> AccessibleCitiesIndex = new();
    public int[] Degrees = new int[9];
    public Dictionary<string, Level> Levels = new();

    public static LevelManager Ins;
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }
    void Start()
    {
        //InitMap();
    }

    private void InitMap()
    {
        Map = transform.GetChild(0).gameObject;

        StartBtn = transform.GetChild(0).Find("StartBtn").GetComponent<Button>();
        //StartBtn.onClick.AddListener(() => GetLevel());
        StartBtn.enabled = false;
        StartBtn.transform.GetComponent<Image>().color = Color.grey;

        MapRoads = transform.GetChild(0).Find("MapRoads");

        Cities = MapRoads.Find("Cities");

        Description = transform.GetChild(0).Find("Description");
        CityNmaeTMP = Description.GetChild(0).GetComponent<TextMeshProUGUI>();
        CityDesctiptionTMP = Description.GetChild(1).GetComponent<TextMeshProUGUI>();

        Pos = transform.GetChild(0).Find("Pos");


        ChooseBg = MapRoads.Find("circles").GetComponentsInChildren<Image>();

        ShowMap();
        ShowAccessibleCities();
        ShowAccessibleRoads();
    }



    public void SetLevel(string name)
    {
        if (!Levels.TryGetValue(name, out Level targetLevel))
        {
            targetLevel = new Level(name);
            Levels[name] = targetLevel;
        }
        CurrentLevel = Levels[name];
    }

    public void ShowMap()
    {
        //if (!MapGenerated)
        //{
        //    int ran;
        //    GameObject city;
        //    for (int i = 0; i < Cities.childCount; i++)
        //    {
                //ran = Random.Range(0, CSVManager.Ins.Cities.Count - 1);
                //city = Cities.GetChild(i).gameObject;
                //city.name = CSVManager.Ins.Cities[ran][1];
                //if(city.name == "希望村") 
                //{
                //    Pos.SetParent(city.transform);
                //    Pos.localPosition = new Vector2(0,30);
                //    Pos.DOLocalMoveY(60,0.5f) .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
                //}
                //city.GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/Icon/"+ CSVManager.Ins.Cities[ran][3]);
                //CityDescription[city.name] = CSVManager.Ins.Cities[ran][2];
                //CSVManager.Ins.Cities.RemoveAt(ran);
        //    }
        //    MapGenerated = true;
        //}
        //else {

        //}
    }

    public void ShowAccessibleCities()
    {
        AccessibleCitiesIndex.Clear();
        int index = 0;//Cities.Find(CurrentCity).GetSiblingIndex();
        MapRoads.Find("Blockcircles").GetChild(index).gameObject.SetActive(false);
        if (index + 3 < 9)
        {
            MapRoads.Find("Blockcircles").GetChild(index + 3).gameObject.SetActive(false);
            AccessibleCitiesIndex.Add(index + 3);
        }
        if (index - 3 > -1)
        {
            MapRoads.Find("Blockcircles").GetChild(index - 3).gameObject.SetActive(false);
            AccessibleCitiesIndex.Add(index - 3);
        }
        if (index % 3 == 0)
        {
            MapRoads.Find("Blockcircles").GetChild(index + 1).gameObject.SetActive(false);
            AccessibleCitiesIndex.Add(index + 1);
        }
        else if (index % 3 == 1)
        {
            MapRoads.Find("Blockcircles").GetChild(index + 1).gameObject.SetActive(false);
            MapRoads.Find("Blockcircles").GetChild(index - 1).gameObject.SetActive(false);
            AccessibleCitiesIndex.Add(index + 1);
            AccessibleCitiesIndex.Add(index - 1);
        }
        else if (index % 3 == 2)
        {
            MapRoads.Find("Blockcircles").GetChild(index - 1).gameObject.SetActive(false);
            AccessibleCitiesIndex.Add(index - 1);
        }
        AccessibleCitiesIndex.Sort();
    }

    private void ShowAccessibleRoads()
    {
        int index = 0;// Cities.Find(CurrentCity).GetSiblingIndex();
        for (int i = 0; i < AccessibleCitiesIndex.Count; i++)
        {
            if (AccessibleCitiesIndex[i] < index)
            {
                MapRoads.Find("Blockroads/"+ AccessibleCitiesIndex[i] + index).gameObject.SetActive(false);
            }
            else
            {
                MapRoads.Find("Blockroads/" + index + AccessibleCitiesIndex[i] ).gameObject.SetActive(false);
            }
        }
    }
}

public class Level
{
    public string Name { get; set; }
    public List<string[]> Events { get; set; }
    public List<string[]> Battles { get; set; }

    public Level(string name)
    {
        Name = name;
        //TODO目前只有一关
        //Events = CSVManager.Ins.GetEventsByArea(name);
        Battles = CSVManager.Ins.GetBattlesByArea(name);
    }
}
