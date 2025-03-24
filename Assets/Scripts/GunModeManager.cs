using System;
using UnityEngine;

public class GunModeManager : MonoBehaviour
{
    //Manager�����õ���ģʽ
    public static GunModeManager Instance { get; private set; }

    [SerializeField] private GameObject Gun;
    [SerializeField] private GameObject GravityGunPrefab;
    [SerializeField] private GameObject TimeGunPrefab;

    private BaseGravityGunMode _currentGravityGunModeScript;
    private BaseTimeGunMode _currentTimeGunModeScript;

    private bool isGravityModeActive = true;

    public delegate void OnGunModeChanged();
    public static event OnGunModeChanged onGunModeChange;

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
        GravityGunPrefab.SetActive(true);
        TimeGunPrefab.SetActive(false);

        onGunModeChange += SwitchGunPrefab;
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
        //����Ұ����Ҽ�ʱ�����õ�ǰģʽ�ű���������һģʽ�ű�
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //ֻ�е��ҽ���ǹеͬʱӵ������ģʽʱ���ܽ���ģʽ�л�
            if(HasGravityAndTimeMode())
            {
                ChangeGunMode();
            }
        }
        //���ɿ��Ҽ�ʱ�л���ԭ����ģʽ
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (HasGravityAndTimeMode())
            {
                ChangeGunMode();
            }
        }
    }

    private void ChangeGunMode()
    {
        isGravityModeActive = !isGravityModeActive;
        SetGunMode(isGravityModeActive);
        onGunModeChange?.Invoke();
    }

    private bool HasGravityAndTimeMode()
    {
        return Gun.transform.GetComponent<BaseGravityGunMode>() != null && Gun.transform.GetComponent<BaseTimeGunMode>() != null;
    }
    
    private void SwitchGunPrefab()
    {
        TimeGunPrefab.SetActive(!isGravityModeActive);
    }
}
