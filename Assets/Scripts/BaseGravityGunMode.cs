using UnityEngine;

public class BaseGravityGunMode : GunMode
{
    [SerializeField] protected float canGrabDistance;
    [SerializeField] protected Transform grabPoint;

    private bool isGrabing = false;
    protected override void HandlerInteraction()
    {
        //仅在按住右键时，这个函数会启用
        RaycastHit hit;
        //仅能与小型物体交互，可以改变物理重力，或"原力"抓取物品，准星瞄准到可互动物品时，物品高亮显示
        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward,out hit, interactionRange))
        {
            if (hit.transform.CompareTag("Small Item"))
            {
                //按键[Q]改变物理重力
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    hit.transform.GetComponent<Item>().ChangeGravity();
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //按键[鼠标左键]使物品飞向玩家
                    hit.transform.GetComponent<Item>().MoveTo(transform);
                    
                }
                if(Input.GetKeyDown (KeyCode.E))
                {
                    //按下E时，当物品距离玩家小于一定距离时 抓取物品
                    if (Vector3.Distance(transform.position, hit.transform.position) < canGrabDistance && !isGrabing)
                    {
                        isGrabing = true;
                        //将物品设为重力枪的子物体，并禁用物体的物理效果
                        hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                        hit.transform.SetParent(transform);

                        hit.transform.position = grabPoint.position;
                        return;
                    }
                    //如果正在抓取物品，再按[E]放下物品
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
        //移除当前脚本，启用Level2脚本
    }

    protected override void Update()
    {
        base.Update();
    }
}
