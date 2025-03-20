using System;
using UnityEngine;

public class GunModeManager : MonoBehaviour
{
    //Manager�����õ���ģʽ
    public static GunModeManager Instance { get; private set; }

    [SerializeField] private GameObject Gun;
    private BaseGravityGunMode _currentGravityGunModeScript;
    private BaseTimeGunMode _currentTimeGunModeScript;

    private bool isGravityModeActive = true;

    //ȷ��Manager��ֻ��һ��ʵ����ȫ�ֿɷ���
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetGunMode(isGravityModeActive);
    }

    private void SetGunMode(bool isGravityMode)
    {
        if (HasGravityAndTimeMode())
        {
            _currentGravityGunModeScript = Gun.transform.GetComponent<BaseGravityGunMode>();
            _currentTimeGunModeScript = Gun.transform.GetComponent<BaseTimeGunMode>();

            _currentGravityGunModeScript.enabled = isGravityMode;
            _currentTimeGunModeScript.enabled = !isGravityMode;
        }
    }


    private void Update()
    {
        //����Ұ���Gʱ�����õ�ǰģʽ�ű���������һģʽ�ű�
        if(Input.GetKeyDown(KeyCode.G))
        {
            //ֻ�е��ҽ���ǹеͬʱӵ������ģʽʱ���ܽ���ģʽ�л�
            if(HasGravityAndTimeMode())
            {
                isGravityModeActive = !isGravityModeActive;
                SetGunMode(isGravityModeActive);
            }
        }
    }

    private bool HasGravityAndTimeMode()
    {
        return Gun.transform.GetComponent<BaseGravityGunMode>() != null && Gun.transform.GetComponent<BaseTimeGunMode>() != null;
    }
    public void ChangeGunMode()
    {
        
    }
}
