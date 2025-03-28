using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // 确保有刚体组件
public class FallingBlock : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.Play3DSound("Block_Impact",transform.position);
    }
}