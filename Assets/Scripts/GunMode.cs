using System;
using UnityEngine;
using UnityEngine.Animations;

public class GunMode : MonoBehaviour
{
    [SerializeField] protected Transform fpsCam;
    [SerializeField] protected float interactionRange;
    [SerializeField] protected float canGrabDistance;
    [SerializeField] protected Transform grabPoint;

    protected virtual void HandlerInteraction() { }

    protected virtual void Update()
    {
        HandlerInteraction();
    }
    protected void GrabLogic(RaycastHit hit)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //按下E时，当物品距离玩家小于一定距离且物品不在时停状态时 抓取物品
            if (Vector3.Distance(transform.position, hit.transform.position) < canGrabDistance && !hit.transform.GetComponent<Item>().IsTimeStopped() && !hit.transform.GetComponent<Item>().IsGrabbed())
            {
                hit.transform.GetComponent<Item>().SetGrab(true);

                //将物品设为重力枪的子物体，并禁用物体的物理效果
                hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.SetParent(transform);

                hit.transform.position = grabPoint.position;
                return;
            }
            //如果正在抓取物品，再按[E]放下物品
            if (hit.transform.GetComponent<Item>().IsGrabbed())
            {
                hit.transform.GetComponent<Item>().SetGrab(false);
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.SetParent(null);
            }
        }
    }

}
