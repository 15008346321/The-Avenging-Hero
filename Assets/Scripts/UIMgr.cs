using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UITop, UIBot,角色栏;
    public static UIMgr Ins;
    public Material 滚动背景;
    public List<布阵UI> 布阵UI列表;
    const float 滚动速度 = 0.1f, 停止速度 = 0f;
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

    public void 角色模拟移动动画()
    {
        for (int i = 0; i < 布阵UI列表.Count; i++)
        {
            //布阵UI列表[i].transform.DOMoveX()
        }
    }

    public void 隐藏小队()
    {
        for (int i = 0; i < 布阵UI列表.Count; i++)
        {
            布阵UI列表[i].gameObject.SetActive(false);
        }
    }

    public void 显示角色栏()
    {
        角色栏.gameObject.SetActive(true);
        TeamManager.Ins.更新角色栏数据();
    }

    public void 收起角色栏()
    {
        角色栏.gameObject.SetActive(false);
    }

    public void 显示小队()
    {
        for (int i = 0; i < TeamManager.Ins.TeamData.Count; i++)
        {
            布阵UI列表[i].gameObject.SetActive(true);
            布阵UI列表[i].unitData = TeamManager.Ins.TeamData[i];
            布阵UI列表[i].角色图片.sprite = TeamManager.Ins.TeamData[i].角色图片;
            布阵UI列表[i].transform.position = BattleMgr.Ins.ourObj.transform.GetChild(布阵UI列表[i].unitData.Cell - 1).position;
        }
    }

    public void 设置拖动的图片到指定布阵栏(UnitData ud)
    {
        for (int i = 0; i < 布阵UI列表.Count; i++)
        {
            if (!布阵UI列表[i].gameObject.activeInHierarchy)
            {
                布阵UI列表[i].角色图片.sprite = ud.角色图片;
                布阵UI列表[i].gameObject.SetActive(true);
                布阵UI列表[i].transform.position = BattleMgr.Ins.ourObj.transform.GetChild(ud.Cell - 1).position;
                布阵UI列表[i].transform.GetComponent<布阵UI>().unitData = ud;
                break;
            }
        }
    }

    //显示UI
}
