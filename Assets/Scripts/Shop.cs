using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button[] GoodsBtns = new Button[8], OperateBtns = new Button[3];
    public TextMeshProUGUI[] GoodsPrice = new TextMeshProUGUI[8];
    public Image[] GoodsImage = new Image[8];
    List<string[]> GoodsList = new();
    int GoodsIndex;
    public Transform GoodsNode;
    public TextMeshProUGUI DetailTitleTMP, DetailContentTMP;
    public static Shop Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    private void Start()
    {
        Init();
        RefreshGoods();
    }

    public void Init()
    {
        DetailTitleTMP   = transform.Find("Detail").GetChild(0).GetComponent<TextMeshProUGUI>();
        DetailContentTMP = transform.Find("Detail").GetChild(1).GetComponent<TextMeshProUGUI>();
        DetailTitleTMP.text = "欢迎光临";
        DetailContentTMP.text = null;

        GoodsNode = transform.Find("Goods");
        for (int i = 0; i < GoodsNode.childCount; i++)
        {
            var j = i;
            GoodsBtns[i] = GoodsNode.GetChild(i).GetComponent<Button>();
            GoodsBtns[i].onClick.AddListener(() => OnGoodsChoose(j));
            GoodsPrice[i] = GoodsBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            GoodsImage[i] = GoodsNode.GetChild(i).GetComponent<Image>();
        }
        var OperateNode = transform.Find("Operate");
        for (int i = 0; i < OperateNode.childCount; i++)
        {
            OperateBtns[i] = OperateNode.GetChild(i).GetComponent<Button>();
        }
        OperateBtns[0].onClick.AddListener(Buy);
        OperateBtns[1].onClick.AddListener(RefreshGoods);
        OperateBtns[2].onClick.AddListener(LeaveOut);
    }
    public void OnGoodsChoose(int j)
    {
        GoodsIndex = j;
        DetailTitleTMP.text   = GoodsList[GoodsIndex][1];
        DetailContentTMP.text = GoodsList[GoodsIndex][3];
    }

    public void RefreshGoods()
    {
        //技能2遗物2属性2药水2
        //BagManager.Ins.Gold -= 2;
        //GoodsList.Clear();
        //var ran = Random.Range(0,CSVManager.Ins.Skills.Count);
        //GoodsList.Add(CSVManager.Ins.Skills[ran]);
        //ran = Random.Range(0, CSVManager.Ins.Skills.Count);
        //GoodsList.Add(CSVManager.Ins.Skills[ran]);

        //var ran1 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //while (BagManager.Ins.GotRelics.Contains(ran1)) ran1 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //GoodsList.Add(CSVManager.Ins.Relics[ran1]);
        //ran = Random.Range(0, CSVManager.Ins.Relics.Count);
        //while (ran == ran1) ran = Random.Range(0, CSVManager.Ins.Relics.Count);
        //while (BagManager.Ins.GotRelics.Contains(ran)) ran = Random.Range(0, CSVManager.Ins.Relics.Count);
        //GoodsList.Add(CSVManager.Ins.Relics[ran]);

        //ran = Random.Range(0, CSVManager.Ins.Goods.Count);
        //GoodsList.Add(CSVManager.Ins.Goods[ran]);
        //ran = Random.Range(0, CSVManager.Ins.Goods.Count);
        //GoodsList.Add(CSVManager.Ins.Goods[ran]);


        //for (int i = 0; i < GoodsList.Count; i++)
        //{
        //    GoodsNode.GetChild(i).gameObject.SetActive(true);
        //    GoodsPrice[i].text = GoodsList[i][4];
        //    if (GoodsList[i][2] == "普攻") GoodsImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/Items/book1");
        //    else if (GoodsList[i][2] == "追打") GoodsImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/Items/book2");
        //    else if (GoodsList[i][2] == "被动") GoodsImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/Items/book3");
        //    else { GoodsImage[i].sprite = Resources.Load<Sprite>(GoodsList[i][6]);}
        //}
    }

    public void Buy()
    {
        if (BagManager.Ins.Gold >= int.Parse(GoodsList[GoodsIndex][4]))
        {
            BagManager.Ins.Gold -= int.Parse(GoodsList[GoodsIndex][4]);
            GoodsNode.GetChild(GoodsIndex).gameObject.SetActive(false);
            DetailTitleTMP.text = "再来点什么";
            DetailContentTMP.text = null;
            //ExecuteMgr.Ins.ExecuteCode(GoodsList[GoodsIndex][5]);

        }
        else
        {
            DetailTitleTMP.text = "冒险家你身上没钱了哟";
            DetailContentTMP.text = null;
        }
    }

    public void LeaveOut()
    {
        gameObject.SetActive(false);
        EventsMgr.Ins.ExploreBtn.gameObject.SetActive(true);
    }
}
    
