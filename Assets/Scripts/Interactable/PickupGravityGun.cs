using UnityEngine;

public class PickupGravityGun : Pickable
{
    //��������ǹʱ����
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GunUpgradeManager.Instance.FirstGetGravityGun();
    }
}
