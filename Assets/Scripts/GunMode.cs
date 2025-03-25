using System;
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
            //����Eʱ������Ʒ�������С��һ����������Ʒ����ʱͣ״̬ʱ ץȡ��Ʒ
            if (Vector3.Distance(transform.position, hit.transform.position) < canGrabDistance && !hit.transform.GetComponent<Item>().IsTimeStopped() && !hit.transform.GetComponent<Item>().IsGrabbed())
            {
                hit.transform.GetComponent<Item>().SetIsGrabbed(true);

                //����Ʒ��Ϊ����ǹ�������壬���������������Ч��
                hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.SetParent(transform);

                hit.transform.position = grabPoint.position;
                return;
            }
            //�������ץȡ��Ʒ���ٰ�[E]������Ʒ
            if (hit.transform.GetComponent<Item>().IsGrabbed())
            {
                hit.transform.GetComponent<Item>().SetIsGrabbed(false);
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.SetParent(null);
            }
        }
    }

}
