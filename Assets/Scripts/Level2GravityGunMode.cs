using System;
using UnityEngine;

public class Level2GravityGunMode : BaseGravityGunMode
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] GameObject player;

    private void Start()
    {
        //二段跳逻辑放在PlayerMovement中，当二级重力枪解锁的时候，解锁二段跳技能
        player.GetComponent<PlayerMovement>().UnlockPushAbility();
    }
    protected override void HandlerInteraction()
    {
        RaycastHit hit;
        //仅能与小型物体交互，可以改变物理重力，或"原力"抓取物品，准星瞄准到可互动物品时，物品高亮显示
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
