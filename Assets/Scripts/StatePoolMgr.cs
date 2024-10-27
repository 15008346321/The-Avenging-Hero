using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePoolMgr : MonoBehaviour
{
    public GameObject Prefab;
    public int PoolMax,SameTimeStateCount;
    public float time;
    public List<State> Pool;
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
            State s = g.GetComponent<State>();
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

    void SetXOffset(State state)
    {
        if (state.transform.GetSiblingIndex() != 0)
        {
            SameTimeStateCount += 1;
            state.transform.localPosition = new Vector2(55 * SameTimeStateCount, 0);
        }
        else
        {
            state.transform.localPosition = Vector2.zero;
        }
    }

    public State Get()
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
        State s = Get();
        s.tmp.text = damage.ToString();
        s.image.sprite = CSVManager.Ins.TypeIcon[type];
        s.transform.SetParent(u.StatePos);
        SetXOffset(s);
        s.gameObject.SetActive(true);
        s.animator.Play("类型伤害");
    }

    public void 状态(Unit u, string text) 
    {
        State s = Get();

        s.tmp.text = text;
        s.transform.SetParent(u.StatePos);
        s.transform.localPosition = Vector2.zero;
        s.gameObject.SetActive(true);
        s.animator.Play("状态");
    }
}

public enum DamageType
{
    物理伤害,
    火元素伤害,
    燃烧伤害,
}
