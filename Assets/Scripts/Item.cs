using Newtonsoft.Json.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Item:MonoBehaviour
{
    [Header("�����ٿ����")]
    [SerializeField] private float launchAngle = 45f;
    private Rigidbody rb;
    private bool isGravityChanged = false;


    [Header("ʱͣ�ٿ����")]
    [SerializeField] private Material timeStoppedMaterial;
    private Material originMaterial;
    private float timer;
    private bool isTimeStopped;

    private bool isGrabbed = false;

    //����ʱͣ�����¼���ί��
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
            //��ʱͣʱ��ʱͣ��������ʼ����
           timer -= Time.deltaTime;
            if(timer < 0)
            {
                //����ʱ�䵽ʱ���������ʱͣ״̬
                CancelTimeStop();
                isTimeStopped = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (isGravityChanged)
        {
            // ���㷴���������� = ���� * �����������ٶ�
            Vector3 reverseGravity = Physics.gravity * -1f * rb.mass;
            rb.AddForce(reverseGravity, ForceMode.Force);
        }
    }

    #region Gravity Control
    public void ChangeGravity()
    {
        if(!isGravityChanged)
        {
            //�ı�����
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
        //������ڷ�ת����״̬������������Ϊ����״̬
        if (isGravityChanged)
        {
            isGravityChanged = false;
            rb.useGravity = true;
        }
        Vector3 launchVelocity = CalculateLaunchVelocity(destination);

        //ForceMode.VelocityChange˲��ı��ٶ�(����������ֱ���޸��ٶ�)
        rb.AddForce(launchVelocity, ForceMode.VelocityChange);
    }
   

    private Vector3 CalculateLaunchVelocity(Transform destination)
    {
        //Ϊʹ�������б���˶������Ϊdestination��ͨ������ʽ���������launchVelocity
        Vector3 displacement = destination.position - transform.position;
        float horizontalDistance = new Vector3(displacement.x, 0, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float angle = launchAngle * Mathf.Deg2Rad;

        // ������ٶ�
        float numerator = gravity * horizontalDistance * horizontalDistance;
        float denominator = 2 * Mathf.Pow(Mathf.Cos(angle), 2) * (horizontalDistance * Mathf.Tan(angle) - verticalDistance);
        float initialSpeed = Mathf.Sqrt(numerator / denominator);

        // �ֽ��ٶȷ���
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
        //�ı����
        ChangeMaterial(timeStoppedMaterial);
        //���ü�������ͬʱ��������Ϊ��ʱͣ״̬
        SetTimeStopTimer(time);
        isTimeStopped = true;

        //������������Ч��
        rb.isKinematic = true;
    }

    public void CancelTimeStop()
    {
        //�����������
        ResetMaterial();
        isTimeStopped = false;

        /*���¼���Ϊ��ʱ����Ӧ��������
         * (����BaseTimeGunMode�ﶩ����onTimeStoppedCanceled�¼������¼���Ϊ��ʱ�����ö���������¼��ĺ�������PlusTimeStopCount())
         * "?."��Ч��ͳд����
         * if (onTimeStoppedCanceled != null) 
         * {
              onTimeStoppedCanceled.Invoke();
           }
         */

        //������������Ч��
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
