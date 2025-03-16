using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private Transform playerBody;

    float xRotation = 0f;
    
    void Start()
    {
        //将指针锁定在屏幕中心并隐藏
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //Unity中"Mouse X"和"Mouse Y"返回的是当前帧的鼠标移动量，这个值已经和帧率有关，不用乘Time.deltatime
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //-=保证上下转动视角与输入不是反转的，使用Clamp限制上下转动角度
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 85f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //使玩家可以左右转动视角
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
