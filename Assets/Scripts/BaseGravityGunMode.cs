using UnityEngine;

public class BaseGravityGunMode : GunMode
{



    protected override void HandlerInteraction()
    {
        RaycastHit hit;
        //������С�����彻�������Ըı�������������"ԭ��"ץȡ��Ʒ��׼����׼���ɻ�����Ʒʱ����Ʒ������ʾ
        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward,out hit, interactionRange))
        {
            if (hit.transform.CompareTag("Small Item"))
            {
                SmallItemInteractionLogic(hit);
            }
            else
                return;
        }
    

    }

    protected void SmallItemInteractionLogic(RaycastHit hit)
    {
        //����[Q]�ı���������
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hit.transform.GetComponent<Item>().ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //����[������]ʹ��Ʒ�������
            hit.transform.GetComponent<Item>().MoveTo(transform);

        }
        GrabLogic(hit);
    }

   

    protected override void Update()
    {
        base.Update();
    }
}
