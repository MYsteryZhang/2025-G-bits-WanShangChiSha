using Newtonsoft.Json.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Item:MonoBehaviour
{
    [Header("重力操控相关")]
    [SerializeField] private float launchAngle = 45f;
    private Rigidbody rb;
    private bool isGravityChanged = false;


    [Header("时停操控相关")]
    [SerializeField] private Material timeStoppedMaterial;
    private Material originMaterial;
    private float timer;
    private bool isTimeStopped;

    private bool isGrabbed = false;

    //定义时停结束事件和委托
    public delegate void OnTimeStoppedCanceled();
    public static event OnTimeStoppedCanceled onTimeStoppedCanceled;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (isTimeStopped)
        {
            //当时停时，时停计数器开始计数
           timer -= Time.deltaTime;
            if(timer < 0)
            {
                //计数时间到时，解除物体时停状态
                CancelTimeStop();
                isTimeStopped = false;
            }
        }
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

    #region Gravity Control
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
    #endregion

    #region Time Stop Control
    private void SetTimeStopTimer(float time)
    {
        timer = time;
    }

    public bool IsTimeStopped()
    {
        return isTimeStopped;
    }
    
    public void TimeStop(float time)
    {
        //改变材质
        ChangeMaterial(timeStoppedMaterial);
        //设置计数器，同时将物体置为已时停状态
        SetTimeStopTimer(time);
        isTimeStopped = true;

        //禁用物体物理效果
        rb.isKinematic = true;
    }

    public void CancelTimeStop()
    {
        //重置物体材质
        ResetMaterial();
        isTimeStopped = false;

        /*当事件不为空时，响应函数调用
         * (即在BaseTimeGunMode里订阅了onTimeStoppedCanceled事件，当事件不为空时，调用订阅了这个事件的函数，即PlusTimeStopCount())
         * "?."等效传统写法：
         * if (onTimeStoppedCanceled != null) 
         * {
              onTimeStoppedCanceled.Invoke();
           }
         */

        //重置物体物理效果
        rb.isKinematic = false;
        onTimeStoppedCanceled?.Invoke();
    }

    public void ChangeMaterial(Material material)
    {
        transform.GetComponent<Renderer>().material = material;
    }

    public void ResetMaterial()
    {
        transform.GetComponent<Renderer>().material = originMaterial;
    }
    #endregion

    public void SetGrab(bool _isGrabbed)
    {
        isGrabbed = _isGrabbed;
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }
}
