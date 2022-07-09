using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillFloor : MonoBehaviour
{
    public Action KillPlayer;
    [SerializeField]
    private int killFloorLevel = -15; 

    private void Update()
    {
        if (gameObject.transform.position.y < killFloorLevel)
            KillPlayer();
    }
}
