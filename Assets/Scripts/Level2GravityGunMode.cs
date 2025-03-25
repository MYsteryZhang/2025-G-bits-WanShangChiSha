using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

public class Level2GravityGunMode : BaseGravityGunMode
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float offsetFactor = 1.2f;//位移参数，控制位移偏量
    [SerializeField] private int maxPushCounts = 7;
    [SerializeField] private GameObject forceArrowPrefab;
    [SerializeField] GameObject player;

    private Vector3 finalForce = Vector3.zero;
    private GameObject forceArrow;
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
        //计算玩家对物体施加推力的方向
        Vector3 direction = hit.transform.position - fpsCam.transform.position;
        //标准化力的方向，使其仅影响方向而不影响力的大小
        direction = Vector3.Normalize(direction);
        //累积最后对物体施加的最终的力
        finalForce += direction * pushForce;
        maxPushCounts--;

        if (forceArrow == null)
        {
            //finalForce.normalized * pushForce / offsetFactor使每次叠加更大的力时，使箭头向力的方向进行位移偏量，避免箭头穿模物体
            forceArrow = Instantiate(forceArrowPrefab, hit.transform.position + finalForce / offsetFactor, Quaternion.identity);
        }
        else 
        {
            //每次施加力都重置力的指示方向
            Destroy(forceArrow);
            forceArrow = Instantiate(forceArrowPrefab, hit.transform.position + finalForce / offsetFactor, Quaternion.identity);
        }

        forceArrow.GetComponent<ForceArrow>().Initialize(finalForce);

        //当物体还在时停状态的时候通过yield return null暂停协程
        while (hit.transform.GetComponent<Item>().IsTimeStopped())
        {
            yield return null;
        }

        //当物体退出时停状态后，执行Push，重置finalForce为零
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
