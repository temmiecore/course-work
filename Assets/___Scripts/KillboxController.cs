using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillboxController : MonoBehaviour
{
    public Collider collider;
    public EnemyController enemyController;
    private bool hasDealtDamage;

    void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        hasDealtDamage = false;
    }

    public void ResetDamageFlag()
    {
        hasDealtDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasDealtDamage)
        {
            GameManager.instance.playerController.TakeDamage(enemyController.damage);
            hasDealtDamage = true; // Why doesn't it work...
        }
    }
}

