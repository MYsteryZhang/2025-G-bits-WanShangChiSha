using UnityEngine;

public class BaseGravityGunMode : GunMode
{
    [SerializeField] protected float canGrabDistance;
    [SerializeField] protected Transform grabPoint;

    private bool isGrabing = false;
    protected override void HandlerInteraction()
    {
        //���ڰ�ס�Ҽ�ʱ���������������
        RaycastHit hit;
        //������С�����彻�������Ըı�������������"ԭ��"ץȡ��Ʒ��׼����׼���ɻ�����Ʒʱ����Ʒ������ʾ
        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward,out hit, interactionRange))
        {
            if (hit.transform.CompareTag("Small Item"))
            {
                //����[Q]�ı���������
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    hit.transform.GetComponent<Item>().ChangeGravity();
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //����[������]ʹ��Ʒ�������
                    hit.transform.GetComponent<Item>().MoveTo(transform);
                    
                }
                if(Input.GetKeyDown (KeyCode.E))
                {
                    //����Eʱ������Ʒ�������С��һ������ʱ ץȡ��Ʒ
                    if (Vector3.Distance(transform.position, hit.transform.position) < canGrabDistance && !isGrabing)
                    {
                        isGrabing = true;
                        //����Ʒ��Ϊ����ǹ�������壬���������������Ч��
                        hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                        hit.transform.SetParent(transform);

                        hit.transform.position = grabPoint.position;
                        return;
                    }
                    //�������ץȡ��Ʒ���ٰ�[E]������Ʒ
                    if (isGrabing)
                    {
                        isGrabing = false;
                        hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                        hit.transform.SetParent(null);
                    }
                }
            }
            else
                return;
        }
    

    }

    protected override void OnUpdate()
    {
        //�Ƴ���ǰ�ű�������Level2�ű�
    }

    protected override void Update()
    {
        base.Update();
    }
}
