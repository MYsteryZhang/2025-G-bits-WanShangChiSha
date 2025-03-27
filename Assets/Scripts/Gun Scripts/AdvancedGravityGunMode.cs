using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

public class AdvancedGravityGunMode : BaseGravityGunMode
{
    [SerializeField] private float pushForce = 7f;
    [SerializeField] private float offsetFactor = 6f;//λ�Ʋ���������λ��ƫ��
    [SerializeField] private int maxPushCounts = 7;
    [SerializeField] private GameObject forceArrowPrefab;
    [SerializeField] protected GameObject player;

    private Vector3 finalForce = Vector3.zero;
    private bool isChangePlayerGravityUnlocked = false;

    public void SetDefaultValue(GameObject _forceArrowPrefab, GameObject _player, Transform _fpsCam, Transform _grabPoint)
    {
        forceArrowPrefab = _forceArrowPrefab;
        player = _player;
        fpsCam = _fpsCam;
        grabPoint = _grabPoint;
    }
    protected virtual void Start()
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

        if (hit.transform.GetComponent<Item>().Arrow == null)
        {
            //finalForce.normalized * pushForce / offsetFactorʹÿ�ε��Ӹ������ʱ��ʹ��ͷ�����ķ������λ��ƫ���������ͷ��ģ����
            hit.transform.GetComponent<Item>().Arrow = Instantiate(forceArrowPrefab, hit.transform.position + finalForce / offsetFactor, Quaternion.identity);
        }
        else 
        {
            //ÿ��ʩ��������������ָʾ����
            Destroy(hit.transform.GetComponent<Item>().Arrow);
            hit.transform.GetComponent<Item>().Arrow = Instantiate(forceArrowPrefab, hit.transform.position + finalForce / offsetFactor, Quaternion.identity);
        }

        hit.transform.GetComponent<Item>().Arrow.GetComponent<ForceArrow>().Initialize(finalForce);

        //�����廹��ʱͣ״̬��ʱ��ͨ��yield return null��ͣЭ��
        while (hit.transform.GetComponent<Item>().IsTimeStopped())
        {
            yield return null;
        }

        //�������˳�ʱͣ״̬��ִ��Push������finalForceΪ��
        PushItem(hit, direction);
        finalForce = Vector3.zero;
        maxPushCounts++;
        Destroy(hit.transform.GetComponent<Item>().Arrow);
    }

    protected override void Update()
    {
        base.Update();
        ReverseGravity();
    }

    private void ReverseGravity()
    {
        //��G��ת�������
        if (Input.GetKeyDown(KeyCode.G) && isChangePlayerGravityUnlocked)
        {
            player.GetComponent<PlayerMovement>().ReverseGravity();
        }
    }
}
