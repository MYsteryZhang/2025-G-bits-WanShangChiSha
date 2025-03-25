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
    [SerializeField] private float groundPushForce = 8f;    // ��������
    [SerializeField] private float airPushForce = 5f;      // ��������

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    [Header("Camera")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector3 velocity;
    public bool isGrounded;
    private bool isPushing;
    private bool isPushUnlocked = false;
    private bool isChangingGravity = false;


    void Update()
    {
        // ������
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isPushing && isPushUnlocked)
        {
            StartCoroutine(ApplyPush());
        }
        HandleMovement();
    }



    private void HandleMovement()
    {
        // ���ô�ֱ�ٶȣ����Ӵ��������ٶȷ����������෴ʱ��
        if (isGrounded && velocity.y * Mathf.Sign(gravity) <= 0)
        {
            velocity.y = -2f * Mathf.Sign(gravity); // ��΢��ȷ����������
        }

        // ������Ծ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // �����������������Ծ�ٶ�
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * Mathf.Abs(gravity)) * -Mathf.Sign(gravity);
        }


        // Ӧ������
        velocity.y += gravity * Time.deltaTime;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //ʹ��Unity���õ�Move������������ƶ�����
        characterController.Move(move * moveSpeed * Time.deltaTime);
        // �ۺ��ƶ���������ֱ�ٶȣ�
        characterController.Move(velocity * Time.deltaTime);
    }

    private IEnumerator ApplyPush()
    {
        
        if (isPushing) yield break;
        isPushing = true;

        float timer = 0f;
        Vector3 pushDir = -playerCamera.transform.forward;

        // ���ݵ���״̬��������
        if (isGrounded)
        {
            pushDir.y = 0; // �����ˮƽ����
            pushDir = pushDir.normalized * groundPushForce;
        }
        else
        {
            pushDir = pushDir.normalized * airPushForce;
        }

        //ʹ��whileѭ����yield return null��������λ����ÿһ֡����ִ�У�ֱ���ۼ�ʱ�䳬��pushDuration
        while (timer < pushDuration)
        {
            characterController.Move(pushDir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        //��Ȼ֧�ֶ���첽��������λ�ƽ����󴥷������¼�:��isPushingΪfalse��
        isPushing = false;
    }

    public void ReverseGravity()
    {
        StartCoroutine(SmoothRotate(180f));
    }

    private IEnumerator SmoothRotate(float targetAngle)
    {
        //���ı������߼�δ���ʱ��ִֹ��֮����߼�
        if(isChangingGravity) yield break;
        gravity = -gravity;
        isChangingGravity = true;

        Quaternion startPlayerRotation = transform.rotation;
        Quaternion targetPlayerRotation = startPlayerRotation * Quaternion.Euler(0, 0, targetAngle);

        float timer = 0f;
        float duration = 0.5f;

        while (timer < duration)
        {
            transform.rotation = Quaternion.Slerp(startPlayerRotation, targetPlayerRotation, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetPlayerRotation;
        isChangingGravity = false;
    }

    public void UnlockPushAbility()
    {
        isPushUnlocked = true;
    }
}
