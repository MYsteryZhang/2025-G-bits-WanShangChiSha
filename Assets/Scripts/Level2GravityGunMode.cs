using System;
using System.Collections;
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

                    //当物品在时停状态的时候，累积动量，当时停结束的时候，执行AddForce
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
        //当物体还在时停状态的时候通过yield return null暂停协程
        while (hit.transform.GetComponent<Item>().IsTimeStopped())
        {
            yield return null;
        }
        //当物体退出时停状态后，执行后面的逻辑
        PushItem(hit, direction);
    }

    protected override void Update()
    {
        base.Update();
    }
}
