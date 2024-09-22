using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour
{
    private Animator animator;
    //private MeshCollider collider;

    private bool isAttacking = false; 
    public float attackCooldown = 0.3f;
    public float damage = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        //collider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {
        isAttacking = true;  
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") {
            EnemyController hitEnemy = other.gameObject.GetComponent<EnemyController>();
            hitEnemy.TakeDamage(damage);
        }
    }
}
