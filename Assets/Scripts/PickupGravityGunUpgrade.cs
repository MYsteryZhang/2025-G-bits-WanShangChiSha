using UnityEngine;

public class PickupGravityGunUpgrade : Interactable
{
    //����������ǹ�������ʱ����
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
        GunUpgradeManager.Instance.UpgradeToAdvancedGravityGun();
    }
}
