using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

public class Level2GravityGunMode : BaseGravityGunMode
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float offsetFactor = 1.2f;//λ�Ʋ���������λ��ƫ��
    [SerializeField] private int maxPushCounts = 7;
    [SerializeField] private GameObject forceArrowPrefab;
    [SerializeField] GameObject player;

    private Vector3 finalForce = Vector3.zero;
    private GameObject forceArrow;
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
                    if (hit.transform.GetComponent<Item>().IsTimeStopped() && maxPushCounts > 0)
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
        //������Ҷ�����ʩ�������ķ���
        Vector3 direction = hit.transform.position - fpsCam.transform.position;
        //��׼�����ķ���ʹ���Ӱ�췽�����Ӱ�����Ĵ�С
        direction = Vector3.Normalize(direction);
        //�ۻ���������ʩ�ӵ����յ���
        finalForce += direction * pushForce;
        maxPushCounts--;

        if (forceArrow == null)
        {
            //finalForce.normalized * pushForce / offsetFactorʹÿ�ε��Ӹ������ʱ��ʹ��ͷ�����ķ������λ��ƫ���������ͷ��ģ����
            forceArrow = Instantiate(forceArrowPrefab, hit.transform.position + finalForce / offsetFactor, Quaternion.identity);
        }
        else 
        {
            //ÿ��ʩ��������������ָʾ����
            Destroy(forceArrow);
            forceArrow = Instantiate(forceArrowPrefab, hit.transform.position + finalForce / offsetFactor, Quaternion.identity);
        }

        forceArrow.GetComponent<ForceArrow>().Initialize(finalForce);

        //�����廹��ʱͣ״̬��ʱ��ͨ��yield return null��ͣЭ��
        while (hit.transform.GetComponent<Item>().IsTimeStopped())
        {
            yield return null;
        }

        //�������˳�ʱͣ״̬��ִ��Push������finalForceΪ��
        PushItem(hit, direction);
        finalForce = Vector3.zero;
        maxPushCounts++;
        Destroy(forceArrow);
    }

    protected override void Update()
    {
        base.Update();
    }
}
