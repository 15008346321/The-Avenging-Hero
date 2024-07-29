using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform u, t;
    public Unit unit;
    public AtkBase skill;
    public string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Skills.json");
    }
    public void testSave()
    {
        //u.DOMove(t.position, 1f);
        unit = BattleMgr.Ins.UnitObj.transform.GetChild(0).GetComponent<Unit>();
        skill = new 斩击(unit);
        string json = JsonUtility.ToJson(skill);

        print(filePath);
        File.WriteAllText(filePath, json);
        BattleMgr.Ins.ShowSkillName(unit, "拔刀斩");
    }

    public void testLoad()
    {
        string json = File.ReadAllText(filePath);
        skill = JsonUtility.FromJson<斩击>(json); 
    }
    public void test1()
    {
        skill.test();
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
