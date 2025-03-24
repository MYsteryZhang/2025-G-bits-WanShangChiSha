using UnityEngine;

public class BaseGravityGunMode : GunMode
{



    protected override void HandlerInteraction()
    {
        RaycastHit hit;
        //仅能与小型物体交互，可以改变物理重力，或"原力"抓取物品，准星瞄准到可互动物品时，物品高亮显示
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
        //按键[Q]改变物理重力
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hit.transform.GetComponent<Item>().ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //按键[鼠标左键]使物品飞向玩家
            hit.transform.GetComponent<Item>().MoveTo(transform);

        }
        GrabLogic(hit);
    }

   

    protected override void Update()
    {
        base.Update();
    }
}
