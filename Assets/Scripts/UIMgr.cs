using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject 资源OBJ, UIBot,角色栏,提示OBJ;
    public Material 滚动背景;
    public Image BG, 地图名横线1, 地图名横线2;
    public Button 村长BTN,提示BTN,离开BTN;
    public TextMeshProUGUI 提示TMP, 地图名,关卡名;

    const float 滚动速度 = 0.1f, 停止速度 = 0f;

    public static UIMgr Ins;

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    public void 前进动画()
    {
        背景滚动();
    }

    public void 背景滚动()
    {
        滚动背景.SetFloat("SpeedX",滚动速度);
    }

    public void 背景停止()
    {
        滚动背景.SetFloat("SpeedX", 停止速度);
    }

    public void 显示角色栏()
    {
        角色栏.gameObject.SetActive(true);
        TeamMgr.Ins.更新角色栏数据();
    }

    public void 收起角色栏()
    {
        角色栏.gameObject.SetActive(false);
    }

    //public void 设置拖动的图片到指定布阵栏(UnitData ud)
    //{
    //    for (int i = 0; i < 布阵UI列表.Count; i++)
    //    {
    //        if (!布阵UI列表[i].gameObject.activeInHierarchy)
    //        {
    //            布阵UI列表[i].角色图片.sprite = ud.角色图片;
    //            布阵UI列表[i].gameObject.SetActive(true);
    //            布阵UI列表[i].transform.position = BattleMgr.Ins.ourObj.transform.GetChild(ud.Cell - 1).position;
    //            布阵UI列表[i].transform.GetComponent<布阵UI>().unitData = ud;
    //            break;
    //        }
    //    }
    //}

    public void 更换地图(string name)
    {
        BG.sprite = CSVMgr.Ins.BgSprites[name];
        地图名.text = name;
        地图名.transform.SetAsLastSibling();
        地图名.DOFade(1, 1f).OnComplete(() => 地图名.DOFade(0, 1f));
        地图名横线1.DOFade(1, 1f).OnComplete(() => 地图名横线1.DOFade(0, 1f));
        地图名横线2.DOFade(1, 1f).OnComplete(() => 地图名横线2.DOFade(0, 1f));
    }

    public void 当点击村长()
    { 
        提示OBJ.gameObject.SetActive(true);
        提示TMP.text = "孩子,临别之前带上这些\n还有...有机会回来看看...";
        提示BTN.onClick.AddListener(获取村长的传家宝);
    }

    public void 获取村长的传家宝()
    {
        资源OBJ.SetActive(true);
        StartCoroutine(BagMgr.Ins.金币变动(20));
        StartCoroutine(BagMgr.Ins.补给变动(30));
        BagMgr.Ins.获得遗物("村长的传家宝");

        提示OBJ.gameObject.SetActive(true);
        提示TMP.text = "你看着村长的传家宝,你知道这并不是多贵重的东西,却对村长很重要...\n你没有说话,再次向村长行跪拜礼之后离开...";
        村长BTN.gameObject.SetActive(false);
        提示BTN.onClick.RemoveListener(获取村长的传家宝);
        离开BTN.gameObject.SetActive(true);
        离开BTN.onClick.AddListener(()=>EventsMgr.Ins.Fade());
    }

    public void 文字提示(string str)
    {
        提示TMP.text = str;
        提示OBJ.gameObject.SetActive(true);
    }

    public string TMP图片化文字(string str)
    {
        string newString = "";
        foreach (char c in str)
        {
            newString += string.Format("<sprite={0}>", c);
        }
        return newString;
    }
}
