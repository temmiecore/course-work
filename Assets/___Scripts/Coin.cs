using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public override void PickUp()
    {
        GameManager.instance.audioManager.Play("player_pickup");
        GameManager.instance.editMoney(5);
        Destroy(gameObject);
    }
}
