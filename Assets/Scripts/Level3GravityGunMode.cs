using UnityEngine;

public class Level3GravityGunMode : Level2GravityGunMode
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandlerInteraction()
    {
        base.HandlerInteraction();
        //��G��ת�������
        if (Input.GetKeyDown(KeyCode.G))
        {
            player.GetComponent<PlayerMovement>().ReverseGravity();
        }
    }


    protected override void Update()
    {
        base.Update();
    }
}
