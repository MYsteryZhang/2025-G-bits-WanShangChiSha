using UnityEngine;

public class GunUpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject gravityGun;
    [SerializeField] private GameObject timeGun;
    public static GunUpgradeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FirstGetGravityGun()
    {
        gravityGun.SetActive(true);
    }

    public void UpgradeGravityGun()
    {

    }
}
