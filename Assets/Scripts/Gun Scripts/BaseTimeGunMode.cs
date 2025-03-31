using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BaseTimeGunMode : GunMode
{
    [SerializeField] private float timeStopDuration = 5f;//时停持续时间
    [SerializeField] private int timeStopCount = 1;//当前可时停物体数量

    private void Start()
    {
        //订阅Item.onTimerLessThanZero事件，当时停物体时停状态解除时，将可时停物体计数++
        Item.onTimeStoppedCanceled += PlusTimeStopCount;
    }

    private void OnDestroy()
    {
        //取消事件订阅避免内存泄漏
        Item.onTimeStoppedCanceled -= PlusTimeStopCount;
    }
    protected override void HandlerInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, interactionRange))
        {
            //初级时停枪只能时停单个物体
            //鼠标左键使物体时停
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //当射线检测物品是Small Item且物体被时停时，解除当前物体时停
                if (hit.transform.CompareTag("Small Item") && hit.transform.GetComponent<Item>().IsTimeStopped())
                {
                    hit.transform.GetComponent<Item>().CancelTimeStop();
                    return;
                }
                //当射线检测物品是Small Item 且物体没有被时停 且当前可时停物体大于0 且物品不被抓取时，时停当前物体
                if(timeStopCount > 0 && hit.transform.CompareTag("Small Item") && !hit.transform.GetComponent<Item>().IsTimeStopped() && !hit.transform.GetComponent<Item>().IsGrabbed())
                {
                    //伪代码:SoundManager.Instance.PlaySFX("TimeStopped")
                    timeStopCount--;
                    hit.transform.GetComponent<Item>().TimeStop(timeStopDuration);
                }
            }
            GrabLogic(hit);
        }
           
    }

   
    protected void PlusTimeStopCount()
    {
        timeStopCount++;
    }


    protected override void Update()
    {
        base.Update();
    }

}
