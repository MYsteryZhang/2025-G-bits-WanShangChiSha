// ForceArrow.cs
using UnityEngine;

public class ForceArrow : MonoBehaviour
{
    [SerializeField] private float maxForceFactor = 5f; // 控制箭头长度的参数
    [SerializeField] private float maxScale = 0.8f;

    private Vector3 forceDirection;
    private float forceMagnitude;

    public void Initialize(Vector3 force)
    {
        forceDirection = force.normalized;
        forceMagnitude = force.magnitude;

        // 调整箭头方向与长度
        transform.rotation = Quaternion.LookRotation(forceDirection);
        float scaleFactor = Mathf.Clamp(forceMagnitude / maxForceFactor, 0.46f, maxScale);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleFactor);

    }
}