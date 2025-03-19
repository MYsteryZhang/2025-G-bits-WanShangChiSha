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
            // ���㷴���������� = ���� * �����������ٶ�
            Vector3 reverseGravity = Physics.gravity * -1f * rb.mass;
            rb.AddForce(reverseGravity, ForceMode.Force);
        }
    }

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

    public void TimeStop()
    {

    }
}
