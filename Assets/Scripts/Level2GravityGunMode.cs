using System;
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
                    hit.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * pushForce, ForceMode.Impulse);
                }
            }
            else
                return;
        }
    }

   

    protected override void Update()
    {
        base.Update();
    }
}
