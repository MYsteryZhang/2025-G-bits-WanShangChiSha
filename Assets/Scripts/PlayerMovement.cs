using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private CharacterController characterController;

    private Vector3 velocity;
    private bool isGrounded;
    // Update is called once per frame
    void Update()
    {
        // ������
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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
}
