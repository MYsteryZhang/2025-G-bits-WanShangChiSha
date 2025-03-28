using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class GunMode : MonoBehaviour
{
    [SerializeField] protected Transform fpsCam;
    [SerializeField] protected float interactionRange = 40f;
    [SerializeField] protected float canGrabDistance = 3f;
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
                hit.transform.GetComponent<Item>().SetIsGrabbed(true);

                //将物品设为重力枪的子物体，并禁用物体的物理效果
                hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.SetParent(transform);
                hit.transform.position = grabPoint.position;
                return;
            }
            //如果正在抓取物品，再按[E]放下物品
            if (hit.transform.GetComponent<Item>().IsGrabbed())
            {
                StartCoroutine(PutDownItem(hit));
            }
        }
    }

    private static IEnumerator PutDownItem(RaycastHit hit)
    {
        hit.transform.GetComponent<Rigidbody>().isKinematic = false;
        hit.transform.GetComponent<Item>().SetIsGrabbed(false);
        //等待一段时间再将物品的父节点设为Null，使玩家松开物品的时候，物品的掉落更符合直觉
        yield return new WaitForSeconds(.2f);
        hit.transform.SetParent(null);


    }
}
