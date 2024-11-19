using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEvent0 : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTriggerForNext() //动画事件
    {
        player.anim.SetBool("canRunning?", true);
    }

    public void AnimationTriggerForNext2() //动画事件
    {
        player.anim.SetBool("canRunning?", false);
    }
}
