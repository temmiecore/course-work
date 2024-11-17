using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player stats")]
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        GameManager.instance.audioManager.Play("player_hit");
        health -= damage; 
        GameManager.instance.editHealth(-damage); // This is weird
    }
}
