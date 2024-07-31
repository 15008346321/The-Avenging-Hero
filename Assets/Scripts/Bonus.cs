using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public TextMeshProUGUI[] BonusTMP =new TextMeshProUGUI[8];
    public List<int> BonusPoints = new();
    public Transform ResultsNode;
    public static Bonus Ins;
    public Button Btn;
    public TextMeshProUGUI GoldTMP;
    private string[] AttrNames = { "力量之石", "火属性石", "水属性石", "风属性石", "雷属性石", "土属性石" };
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    public void Init()
    {
        GoldTMP = GameObject.Find("UI/Top/Gold/TMP").GetComponent<TextMeshProUGUI>();
        ResultsNode = transform.Find("BG/Results");
        Btn = transform.Find("Btn").GetComponent<Button>();
        Btn.onClick.AddListener(AddToBag);
        for (int i = 0; i < ResultsNode.childCount; i++)
        {
            BonusTMP[i] = ResultsNode.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }

    public void AddToBag()
    {
        BagManager.Ins.Gold += BonusPoints[0];
        GoldTMP.text = "" + BagManager.Ins.Gold;
        //TODO按国家获取荣誉，目前战时只管火之国的
        BagManager.Ins.ReptFire += BonusPoints[1];

        for (int i = 2; i < BonusPoints.Count; i++)
        {
            if (BonusPoints[i] > 0)
            {
                BagManager.Ins.BagLt.Add(new Item(AttrNames[i - 2], BonusPoints[i], false));
            }
        }
        transform.gameObject.SetActive(false);
    }



    //战斗完成后调用
    public void ShowBonus()
    {
        string[] monsters = EventsMgr.Ins.monsters.Split('&');
        BonusPoints.Clear();
        //给每个位置先加个0，避免idx报错
        for (int i = 0; i < 8; i++)
        {
            BonusPoints.Add(0);
        }
        for (int i = 0; i < 3; i++)
        {
            var ran = Random.Range(0, monsters.Length-1);
            var mName = monsters[ran].Split('P')[0];
            var bonusCode = CSVManager.Ins.Units[mName][13];
            var AttrName = bonusCode.Split('+')[0];
            BonusPoints[0] += int.Parse(bonusCode.Split('+')[1]);
            BonusPoints[1] += int.Parse(bonusCode.Split('+')[2]);
            if (AttrName == "Atk")          BonusPoints[2] += 1;
            else if (AttrName == "Fire")    BonusPoints[3] += 1;
            else if (AttrName == "Water")   BonusPoints[4] += 1;
            else if (AttrName == "Wind")    BonusPoints[5] += 1;
            else if (AttrName == "Thunder") BonusPoints[6] += 1;
            else if (AttrName == "Earth")   BonusPoints[7] += 1;
        }

        for (int i = 0; i < BonusPoints.Count; i++)
        {
            BonusTMP[i].text = BonusPoints[i].ToString();
            BonusTMP[i].transform.parent.gameObject.SetActive(false);
            if (BonusPoints[i] > 0)
            {
                BonusTMP[i].transform.parent.gameObject.SetActive(true);
            }
        }

        transform.gameObject.SetActive(true);
    }
}
    
