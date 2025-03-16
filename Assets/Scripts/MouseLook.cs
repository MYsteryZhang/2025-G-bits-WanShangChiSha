using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private Transform playerBody;

    float xRotation = 0f;
    
    void Start()
    {
        //��ָ����������Ļ���Ĳ�����
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //Unity��"Mouse X"��"Mouse Y"���ص��ǵ�ǰ֡������ƶ��������ֵ�Ѿ���֡���йأ����ó�Time.deltatime
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //-=��֤����ת���ӽ������벻�Ƿ�ת�ģ�ʹ��Clamp��������ת���Ƕ�
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 85f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //ʹ��ҿ�������ת���ӽ�
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
