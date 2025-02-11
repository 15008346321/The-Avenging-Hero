using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePoolMgr : MonoBehaviour
{
    public GameObject Prefab;
    public int PoolMax,SameTimeStateCount;
    public float time;
    public List<伤害飘字> Pool;
    public Unit 上一个调用者;
    public Transform 提示信息父节点;

    public static StatePoolMgr Ins;

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }
    void Start()
    {
        for (int i = 0; i < PoolMax; i++)
        {
            GameObject g = Instantiate(Prefab);
            伤害飘字 s = g.GetComponent<伤害飘字>();
            s.transform.SetParent(提示信息父节点);
            s.transform.localPosition = Vector3.zero;
            s.gameObject.SetActive(false);
            Pool.Add(s);
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time>1)
        {
            SameTimeStateCount = 0;
        }
    }

    bool 检查是否需要延迟播放(伤害飘字 state)
    {
        if(state.调用者 == 上一个调用者)
        {
            return true;
        }
        上一个调用者 = state.调用者;
        return false;
    }

    public 伤害飘字 Get()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (!Pool[i].gameObject.activeInHierarchy)
            {
                return Pool[i];
            }
        }
        return null;
    }

    public void 类型伤害(Unit u, float damage, string type)
    {
        伤害飘字 s = Get();
        s.调用者 = u;
        s.tmp.text = damage.ToString();
        s.image.sprite = CSVMgr.Ins.TypeIcon[type];

        s.transform.SetParent(u.伤害飘字父节点);

        s.transform.localPosition = Vector2.zero;
        float delay = 0;
        if(检查是否需要延迟播放(s))
        {
            delay = 0.3f;
        };

        StartCoroutine(延迟播放(s, "类型伤害", delay));
    }

    public void 状态(Unit u, string text) 
    {
        伤害飘字 s = Get();
        s.调用者 = u;
        s.tmp.text = text;
        s.transform.SetParent(u.伤害飘字父节点);
        s.transform.localPosition = Vector2.zero;
        float delay = 0;
        if (检查是否需要延迟播放(s))
        {
            delay = 0.3f;
        };

        StartCoroutine(延迟播放(s, "状态", delay));
    }
    public IEnumerator 延迟播放(伤害飘字 s,string 类型 ,float delay) 
    { 
        yield return new WaitForSeconds(delay);
        s.gameObject.SetActive(true);
        s.animator.Play(类型);
    }

}

public enum DamageType
{
    攻击伤害,
    技能伤害,
    异常伤害,
}

public enum ElementType
{
    物理伤害,
    火元素伤害,
    土元素伤害,
    燃烧伤害,
    出血伤害,
    中毒伤害,
}
