using UnityEngine;

public class PickupGravityGunUpgrade : Pickable
{
    //����������ǹ�������ʱ����
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GunUpgradeManager.Instance.UpgradeToAdvancedGravityGun();
    }
}
