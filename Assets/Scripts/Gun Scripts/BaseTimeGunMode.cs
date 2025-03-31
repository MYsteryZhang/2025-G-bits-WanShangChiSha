using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BaseTimeGunMode : GunMode
{
    [SerializeField] private float timeStopDuration = 5f;//ʱͣ����ʱ��
    [SerializeField] private int timeStopCount = 1;//��ǰ��ʱͣ��������

    private void Start()
    {
        //����Item.onTimerLessThanZero�¼�����ʱͣ����ʱͣ״̬���ʱ������ʱͣ�������++
        Item.onTimeStoppedCanceled += PlusTimeStopCount;
    }

    private void OnDestroy()
    {
        //ȡ���¼����ı����ڴ�й©
        Item.onTimeStoppedCanceled -= PlusTimeStopCount;
    }
    protected override void HandlerInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, interactionRange))
        {
            //����ʱͣǹֻ��ʱͣ��������
            //������ʹ����ʱͣ
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //�����߼����Ʒ��Small Item�����屻ʱͣʱ�������ǰ����ʱͣ
                if (hit.transform.CompareTag("Small Item") && hit.transform.GetComponent<Item>().IsTimeStopped())
                {
                    hit.transform.GetComponent<Item>().CancelTimeStop();
                    return;
                }
                //�����߼����Ʒ��Small Item ������û�б�ʱͣ �ҵ�ǰ��ʱͣ�������0 ����Ʒ����ץȡʱ��ʱͣ��ǰ����
                if(timeStopCount > 0 && hit.transform.CompareTag("Small Item") && !hit.transform.GetComponent<Item>().IsTimeStopped() && !hit.transform.GetComponent<Item>().IsGrabbed())
                {
                    //α����:SoundManager.Instance.PlaySFX("TimeStopped")
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
