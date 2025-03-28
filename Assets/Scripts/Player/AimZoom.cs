using UnityEngine;

public class AimZoom : MonoBehaviour
{
    [Header("相机设置")]
    [SerializeField] private Camera playerCamera; // 玩家的主相机
    [SerializeField] private float zoomFOV = 40f; // 瞄准时的视野
    [SerializeField] private float normalFOV = 70f; // 默认视野
    [SerializeField] private float zoomSpeed = 10f; // 视野调整速度

    private bool isAiming = false;

    void Update()
    {
        HandleAimInput();
        AdjustCameraFOV();
    }

    // 检测右键输入
    private void HandleAimInput()
    {
        if (Input.GetMouseButtonDown(1))
        { // 右键按下
            isAiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        { // 右键释放
            isAiming = false;
        }
    }

    // 平滑调整视野
    private void AdjustCameraFOV()
    {
        float targetFOV = isAiming ? zoomFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            zoomSpeed * Time.deltaTime
        );
    }
}
