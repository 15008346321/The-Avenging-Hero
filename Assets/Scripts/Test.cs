using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform u, t;
    public GameObject gaer;
    public Unit unit;
    public ComponentBase skill, AtkSkill1, AtkSkill2;
    public string filePath;
    public bool b;
    public List<string[]> data = new();
    public List<GameObject> GL = new();
    public Dictionary<int, int> id = new Dictionary<int, int>();

    public void Atk999()
    {
        foreach (var item in BattleMgr.Ins.玩家阵营单位列表)
        {
            item.Atk = 999;
            item.Speed = 99;
        }
        EventsMgr.Ins.Gold += 100;
    }
    //public void Test13()
    //{
    //    unit.Anim.Play("idle");
    //}
    //public void Test12()
    //{
    //    unit.Anim.Play("atk");
    //}

    //public void Test10()
    //{
    //    TeamManager.Ins.TagNodes[0][0].text = "aa";
    //    print(TeamManager.Ins.TeamData[0].Tags[0]);
    //    TeamManager.Ins.TagNodes[0][0].text = TeamManager.Ins.TeamData[0].Tags[0];
    //}

    //public void Test11()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        id[i] = i;
    //    }
    //    print(id.Max(a=>a.Value));
    //}
    //public void Test9()
    //{
    //    TextAsset ta = Resources.Load<TextAsset>("Configs/Battles/后山");

    //    string csv = ta.text;

    //    StringReader reader = new StringReader(csv);
    //    //去掉第一行
    //    reader.ReadLine();

    //    string line;

    //    List<string[]> data = new();

    //    while ((line = reader.ReadLine())!=null)
    //    {
    //        string[] values = line.Split(',');
    //        data.Add(values);
    //    }
    //}
    //public void Test6()
    //{
    //    BagManager.Ins.GenerateGear("冒险剑");
    //}

    //public void Test7()
    //{
    //    BagManager.Ins.GenerateGear("气血铠甲");
    //}

    //public void Test8()
    //{
    //    BagManager.Ins.GenerateGear("史莱姆屏障");
    //}

    //private void Start()
    //{
    //    filePath = Path.Combine(Application.persistentDataPath, "Skills.json");
    //}
    //public void testSave()
    //{
    //    //u.DOMove(t.position, 1f);
    //    unit = BattleMgr.Ins.UnitObj.transform.GetChild(0).GetComponent<Unit>();
    //    //skill = new 斩击(unit);
    //    string json = JsonUtility.ToJson(skill);

    //    print(filePath);
    //    File.WriteAllText(filePath, json);
    //    BattleMgr.Ins.ShowSkillName(unit, "拔刀斩");
    //}

    //public void testLoad()
    //{
    //    //string json = File.ReadAllText(filePath);
    //    //string a = "斩击";
    //    //Type t = Type.GetType(a);
    //    //skill = JsonUtility.FromJson<t>(json); 

    //}
    //测试实例化子类
    //public void Test1()
    //{

    //    AtkSkill1 = Activator.CreateInstance(Type.GetType("火球")) as AtkBase;
    //    AtkSkill2 = Activator.CreateInstance(Type.GetType("斩击")) as AtkBase;
    //}
    //public void Test2()
    //{
    //    AtkSkill1.Test();
    //}
    //public void Test3()
    //{
    //    AtkSkill2.Test();
    //}

    //public void test1()
    //{
    //    StartCoroutine(test2());
    //}

    //public IEnumerator test2()
    //{
    //    GameObject g = BattleMgr.Ins.GetUnitObj("初级剑士");
    //    yield return new WaitForSeconds(2);
    //    Unit u = Instantiate(g).transform.GetChild(0).GetComponent<Unit>();
    //}

    //测试避免重复调用
    //public void test4()
    //{
    //    test5();
    //    test5();
    //}
    //public void test5()
    //{
    //    print("tset5 start");
    //    if(b == true) return;
    //    b =true;
    //    print("test5 end");
    //}
}
