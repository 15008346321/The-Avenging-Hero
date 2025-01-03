using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 角色栏拖拽 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnitData unitData;
    public bool isDragging = false; // 标记是否正在拖放
    public Image 公用的拖动图片,角色栏图片; // 用于记录拖放过程中实例化出来的角色图片对象


    private void Awake()
    {
        角色栏图片 = transform.GetChild(0).GetComponent<Image>();
    }
    private void Update()
    {
        if (isDragging && 公用的拖动图片 != null)
        {
            //Vector3 globalMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
            公用的拖动图片.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        print("OnPointerDown");
        if (!isDragging && 角色栏图片.enabled != false)
        {
            if(神像管理器.Ins.当前正在神像)
            {
                if (unitData.在神像位置 != -1)
                {
                    神像管理器.Ins.PrayPosImgs[unitData.在神像位置].gameObject.SetActive(false);
                    unitData.在神像位置 = -1;
                }
            }
            else if(BattleMgr.Ins.当前正在布阵)
            {
                if (TeamMgr.Ins.TeamData.Exists(ud => ud == unitData))
                {
                    UIMgr.Ins.布阵UI列表.Find(UI => UI.unitData == unitData).gameObject.SetActive(false);
                }

            }

            公用的拖动图片.gameObject.SetActive(true);
            公用的拖动图片.sprite = unitData.角色图片;

            isDragging = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging && BattleMgr.Ins.当前正在布阵)
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            //拖到了格子上
            if (hit.collider != null && hit.collider.gameObject.CompareTag("布阵位置"))
            {
                UnitData ud = TeamMgr.Ins.TeamData.Find(td => td.Cell == (hit.collider.gameObject.transform.GetSiblingIndex() + 1));

                if (ud != null)
                {
                    //如果格子上有和this.unitdata不同的数据了 就移除
                    if (ud != unitData)
                    {
                        TeamMgr.Ins.TeamData.Remove(ud);
                        UIMgr.Ins.布阵UI列表.Find(img => img.角色图片.sprite == ud.角色图片).gameObject.SetActive(false);
                    }
                    else
                    {

                    }
                }
                //已经放了四个了 并且this.unitdata在已知四个之外 就不管
                else if (TeamMgr.Ins.TeamData.Count == 4 && !TeamMgr.Ins.TeamData.Exists(ud=>ud==unitData))
                {
                    isDragging = false;
                    公用的拖动图片.gameObject.SetActive(false);
                    return;
                }
                //到空位置并且没满四个

                //新的就加进TeamData
                if (!TeamMgr.Ins.TeamData.Exists(ud=>ud == unitData))
                {
                    TeamMgr.Ins.TeamData.Add(unitData);
                }

                unitData.Cell = hit.collider.gameObject.transform.GetSiblingIndex() + 1;

                UIMgr.Ins.设置拖动的图片到指定布阵栏(unitData);
                
            }
            //没拖到格子上
            else
            {
                if (TeamMgr.Ins.TeamData.Exists(ud => ud == unitData))
                {
                    TeamMgr.Ins.TeamData.Remove(unitData);
                }
            }
            isDragging = false;
            公用的拖动图片.gameObject.SetActive(false);
        }
        if (isDragging && 神像管理器.Ins.当前正在神像)
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            //拖到了神像格子上
            if (hit.collider != null && hit.collider.gameObject.CompareTag("PrayPos"))
            {
                var idx = hit.collider.transform.GetSiblingIndex();
                var 已存在的数据 = TeamMgr.Ins.拥有角色数据.Find(u => u.在神像位置 == idx);
                if(已存在的数据!=null) 已存在的数据.在神像位置 = -1;
                unitData.在神像位置 = idx;
                神像管理器.Ins.PrayPosImgs[idx].gameObject.SetActive(true);
                神像管理器.Ins.PrayPosImgs[idx].sprite = unitData.角色图片;
            }

            isDragging = false;
            公用的拖动图片.gameObject.SetActive(false);
        }
    }
}
