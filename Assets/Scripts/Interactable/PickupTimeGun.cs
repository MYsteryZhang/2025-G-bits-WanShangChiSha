using UnityEngine;

public class PickupTimeGun : Pickable
{
    //������ʱ��ǹʱ����
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GunUpgradeManager.Instance.FirstGetTimeGun();
    }

}
