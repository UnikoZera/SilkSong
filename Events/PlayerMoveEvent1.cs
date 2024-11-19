using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEvent1 : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTriggerForClose() //动画事件
    {
        player.anim.SetBool("canRunning?", false);
    }
}
