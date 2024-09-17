using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public override void PickUp()
    {
        GameManager.instance.editMoney(5);
        Destroy(gameObject);
    }
}
