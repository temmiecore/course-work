using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        player = GameManager.instance.playerController;
    }


    void Update()
    {
        transform.position = player.transform.position;
    }
}
