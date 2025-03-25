using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float pushDuration = 0.3f;
    [SerializeField] private float groundPushForce = 8f;    // 地面推力
    [SerializeField] private float airPushForce = 5f;      // 空中推力

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isPushing;
    private bool isPushUnlocked = false;


    void Update()
    {
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isPushing && isPushUnlocked)
        {
            StartCoroutine(ApplyPush());
        }
        HandleMovement();
    }



    private void HandleMovement()
    {
        // 重置垂直速度（当接触地面且下落时）
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 轻微向下力，确保紧贴地面
        }

        // 处理跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 应用重力
        velocity.y += gravity * Time.deltaTime;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //使用Unity内置的Move方法进行玩家移动操作
        characterController.Move(move * moveSpeed * Time.deltaTime);
        // 综合移动（包含垂直速度）
        characterController.Move(velocity * Time.deltaTime);
    }

    private IEnumerator ApplyPush()
    {
        
        if (isPushing) yield break;
        isPushing = true;

        float timer = 0f;
        Vector3 pushDir = -playerCamera.transform.forward;

        // 根据地面状态调整方向
        if (isGrounded)
        {
            pushDir.y = 0; // 地面仅水平方向
            pushDir = pushDir.normalized * groundPushForce;
        }
        else
        {
            pushDir = pushDir.normalized * airPushForce;
        }

        //使用while循环和yield return null，可以让位移在每一帧中逐步执行，直到累计时间超过pushDuration
        while (timer < pushDuration)
        {
            characterController.Move(pushDir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        //天然支持多段异步操作（如位移结束后触发其他事件:置isPushing为false）
        isPushing = false;
    }

    public void UnlockPushAbility()
    {
        isPushUnlocked = true;
    }
}
