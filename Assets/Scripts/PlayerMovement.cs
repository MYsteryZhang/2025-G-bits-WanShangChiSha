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
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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
}
