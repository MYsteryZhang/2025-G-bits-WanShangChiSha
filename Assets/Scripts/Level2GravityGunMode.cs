using System;
using System.Collections;
using UnityEngine;

public class Level2GravityGunMode : BaseGravityGunMode
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] GameObject player;


    private void Start()
    {
        //�������߼�����PlayerMovement�У�����������ǹ������ʱ�򣬽�������������
        player.GetComponent<PlayerMovement>().UnlockPushAbility();
    }
    protected override void HandlerInteraction()
    {
        RaycastHit hit;
        //������С�����彻�������Ըı�������������"ԭ��"ץȡ��Ʒ��׼����׼���ɻ�����Ʒʱ����Ʒ������ʾ
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, interactionRange))
        {
            if (hit.transform.CompareTag("Small Item"))
            {
                SmallItemInteractionLogic(hit);

            }
            if(hit.transform.CompareTag("Small Item") || hit.transform.CompareTag("Big Item"))
            {
                if (Input.GetKeyDown(KeyCode.T))
                {

                    //����Ʒ��ʱͣ״̬��ʱ���ۻ���������ʱͣ������ʱ��ִ��AddForce
                    if (hit.transform.GetComponent<Item>().IsTimeStopped())
                    {
                        StartCoroutine(PushItemWhenTimeStopCanceled(hit));
                    }
                    else
                    {
                        Vector3 direction = hit.transform.position - fpsCam.transform.position;
                        direction = Vector3.Normalize(direction);
                        PushItem(hit, direction);
                    }
                }
            }
            else
                return;
        }

    }

    private void PushItem(RaycastHit hit, Vector3 _direction)
    {
        hit.transform.GetComponent<Rigidbody>().AddForce(_direction * pushForce, ForceMode.Impulse);
    }

    protected IEnumerator PushItemWhenTimeStopCanceled(RaycastHit hit)
    {
        Vector3 direction = hit.transform.position - fpsCam.transform.position;
        direction = Vector3.Normalize(direction);
        //�����廹��ʱͣ״̬��ʱ��ͨ��yield return null��ͣЭ��
        while (hit.transform.GetComponent<Item>().IsTimeStopped())
        {
            yield return null;
        }
        //�������˳�ʱͣ״̬��ִ�к�����߼�
        PushItem(hit, direction);
    }

    protected override void Update()
    {
        base.Update();
    }
}
