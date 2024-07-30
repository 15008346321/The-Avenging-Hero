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
    public Unit unit;
    public AtkBase skill, AtkSkill1, AtkSkill2;
    public string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Skills.json");
    }
    public void testSave()
    {
        //u.DOMove(t.position, 1f);
        unit = BattleMgr.Ins.UnitObj.transform.GetChild(0).GetComponent<Unit>();
        //skill = new 斩击(unit);
        string json = JsonUtility.ToJson(skill);

        print(filePath);
        File.WriteAllText(filePath, json);
        BattleMgr.Ins.ShowSkillName(unit, "拔刀斩");
    }

    public void testLoad()
    {
        //string json = File.ReadAllText(filePath);
        //string a = "斩击";
        //Type t = Type.GetType(a);
        //skill = JsonUtility.FromJson<t>(json); 

    }
    public void Test1()
    {

        AtkSkill1 = Activator.CreateInstance(Type.GetType("火球")) as AtkBase;
        AtkSkill2 = Activator.CreateInstance(Type.GetType("斩击")) as AtkBase;
    }
    public void Test2()
    {
        AtkSkill1.Test();
    }
    public void Test3()
    {
        AtkSkill2.Test();
    }

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
}
