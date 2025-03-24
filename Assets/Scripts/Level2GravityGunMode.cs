using System;
using System.Collections;
using UnityEngine;

public class Level2GravityGunMode : BaseGravityGunMode
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] GameObject player;

    private RaycastHit hit;

    private void Start()
    {
        //�������߼�����PlayerMovement�У�����������ǹ������ʱ�򣬽�������������
        player.GetComponent<PlayerMovement>().UnlockPushAbility();
    }
    protected override void HandlerInteraction()
    {
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
                        StartCoroutine(PushItemWhenTimeStopCanceled());
                    }
                    else
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * pushForce, ForceMode.Impulse);
                    }
                }
            }
            else
                return;
        }

    }

    protected IEnumerator PushItemWhenTimeStopCanceled()
    {
        //�����廹��ʱͣ״̬��ʱ��ͨ��yield return null��ͣЭ��
        while (hit.transform.GetComponent<Item>().IsTimeStopped())
        {
            yield return null;
        }
        //�������˳�ʱͣ״̬��ִ�к�����߼�
        hit.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * pushForce, ForceMode.Impulse);
    }

    protected override void Update()
    {
        base.Update();
    }
}
