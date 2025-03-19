using Newtonsoft.Json.Serialization;
using UnityEngine;

public class Item:MonoBehaviour
{
    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float launchAngle = 45f;

    private Rigidbody rb;
    private bool isGravityChanged = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isGravityChanged)
        {
            // 计算反向重力：力 = 质量 * 反向重力加速度
            Vector3 reverseGravity = Physics.gravity * -1f * rb.mass;
            rb.AddForce(reverseGravity, ForceMode.Force);
        }
    }

    public void ChangeGravity()
    {
        if(!isGravityChanged)
        {
            //改变重力
            rb.useGravity = false;
            isGravityChanged = true;
        }
        else
        {
            rb.useGravity = true;
            isGravityChanged = false;
        }
    }

    public void MoveTo(Transform destination)
    {
        //如果处于反转重力状态，则重置重力为正常状态
        if (isGravityChanged)
        {
            isGravityChanged = false;
            rb.useGravity = true;
        }
        Vector3 launchVelocity = CalculateLaunchVelocity(destination);

        //ForceMode.VelocityChange瞬间改变速度(忽略质量，直接修改速度)
        rb.AddForce(launchVelocity, ForceMode.VelocityChange);
    }
   

    private Vector3 CalculateLaunchVelocity(Transform destination)
    {
        //为使物体进行斜抛运动后落点为destination，通过物理公式计算所需的launchVelocity
        Vector3 displacement = destination.position - transform.position;
        float horizontalDistance = new Vector3(displacement.x, 0, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float angle = launchAngle * Mathf.Deg2Rad;

        // 计算初速度
        float numerator = gravity * horizontalDistance * horizontalDistance;
        float denominator = 2 * Mathf.Pow(Mathf.Cos(angle), 2) * (horizontalDistance * Mathf.Tan(angle) - verticalDistance);
        float initialSpeed = Mathf.Sqrt(numerator / denominator);

        // 分解速度方向
        Vector3 horizontalDir = new Vector3(displacement.x, 0, displacement.z).normalized;
        Vector3 velocityXZ = horizontalDir * initialSpeed * Mathf.Cos(angle);
        float velocityY = initialSpeed * Mathf.Sin(angle);
        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;
        return launchVelocity;
    }

    public void TimeStop()
    {

    }
}
