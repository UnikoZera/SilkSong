using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEvent2 : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTriggerForEnd() //动画事件
    {
        player.stateMachine.ChangeState(player.idleState);
    }
}
