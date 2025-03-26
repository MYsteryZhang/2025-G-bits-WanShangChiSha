using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // 确保有刚体组件
public class FallingBlock : MonoBehaviour
{
    [SerializeField] private float fallSpeedThreshold = 2f; // 触发音效的最小下落速度


    void Start()
    {

    }

    void Update()
    {

    }



    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.Play3DSound("Block_Impact",transform.position);
    }
}