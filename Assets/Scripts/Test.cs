using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System.Reflection;
using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform u, t;
    public GameObject gaer;
    public Unit unit;
    public ComponentBase skill, AtkSkill1, AtkSkill2;
    public string filePath;
    public bool b;

    public void Test6()
    {
        BagManager.Ins.GenerateGear("冒险剑");
    }

    public void Test7()
    {
        BagManager.Ins.GenerateGear("气血铠甲");
    }

    public void Test8()
    {
        BagManager.Ins.GenerateGear("史莱姆屏障");
    }
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
