using UnityEngine;

public class PickupGravityGun : Interactable
{
    //��������ǹʱ����
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
        GunUpgradeManager.Instance.FirstGetGravityGun();
    }
}
