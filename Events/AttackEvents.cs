using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvents : MonoBehaviour
{
    private Player player;
    // public Collider trigger1 = Collider.GetComponentInChildren<Collider>();
    // Collider trigger2 = GetComponentInChildren<Collider>();
    void Start()
    {
        player = GetComponentInParent<Player>();

    }

    void AttackBegin()
    {
        
    }
}
