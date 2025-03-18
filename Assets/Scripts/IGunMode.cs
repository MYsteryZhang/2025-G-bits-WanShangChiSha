using UnityEngine;

public interface IGunMode
{
    void OnTrigger(RaycastHit target);
    void OnUpdate();
}
