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

    private Vector3 velocity;
    private bool isGrounded;
    private bool isPushing;
    private bool isPushUnlocked = false;


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
        // ���ô�ֱ�ٶȣ����Ӵ�����������ʱ��
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ��΢��������ȷ����������
        }

        // ������Ծ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

    public void UnlockPushAbility()
    {
        isPushUnlocked = true;
    }
}
