using UnityEngine;

public class AimZoom : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] private Camera playerCamera; // ��ҵ������
    [SerializeField] private float zoomFOV = 40f; // ��׼ʱ����Ұ
    [SerializeField] private float normalFOV = 70f; // Ĭ����Ұ
    [SerializeField] private float zoomSpeed = 10f; // ��Ұ�����ٶ�

    private bool isAiming = false;

    void Update()
    {
        HandleAimInput();
        AdjustCameraFOV();
    }

    // ����Ҽ�����
    private void HandleAimInput()
    {
        if (Input.GetMouseButtonDown(1))
        { // �Ҽ�����
            isAiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        { // �Ҽ��ͷ�
            isAiming = false;
        }
    }

    // ƽ��������Ұ
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
