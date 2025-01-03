using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inn : MonoBehaviour
{
    public Image[] 头像;
    public TextMeshProUGUI[] 价格;
    public Button LeaveBtn;
    public Button[] 招募按钮;
    public List<GameObject> 角色;
    public List<UnitData> 角色数据 = new();
    public List<int> 已拥有角色编号 = new();
    public List<int> 已随机到角色编号 = new();
    public GameObject 酒馆;
    public static Inn Ins;
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);

        招募按钮[0].onClick.AddListener(() => 招募(0));
        招募按钮[1].onClick.AddListener(() => 招募(1));
        招募按钮[2].onClick.AddListener(() => 招募(2));
        招募按钮[3].onClick.AddListener(() => 招募(3));
    }

    public void 展示可招募单位()
    {
        酒馆.SetActive(true);
        随机四个角色();
        for (int i = 0; i < 4; i++)
        {
            头像[i].sprite = 角色数据[i].角色图片;
            价格[i].text = "<sprite=\"coin\" name=\"coin\">" + 角色数据[i].角色价格.ToString();
        }
    }

    public void 随机四个角色()
    {
        角色数据.Clear();
        for (int i = 0; i < 4; i++)
        {
            int ran = Random.Range(0, CSVMgr.Ins.Units.Count);
            while (已随机到角色编号.Contains(ran))
            {
                ran = Random.Range(0, CSVMgr.Ins.Units.Count);
            }
            已随机到角色编号.Add(ran);
            角色数据.Add(new UnitData(CSVMgr.Ins.Units[CSVMgr.Ins.角色名称数组[ran]]));
        }
    }

    public void 招募(int index) 
    {
        if (BagMgr.Ins.玩家拥有的金币 > 角色数据[index].角色价格) 
        {
            BagMgr.Ins.玩家拥有的金币 -= 角色数据[index].角色价格;
            角色[index].SetActive(false);
            TeamMgr.Ins.拥有角色数据.Add(角色数据[index]);
        }
    }

    public void OnClickLeaveBtn()
    {
        酒馆.SetActive(false);
        EventsMgr.Ins.是否去往酒馆 = false;
        EventsMgr.Ins.Fade();
    }
}
