using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour
{
    private Animator animator;
    public float lightDamage = 5f;
    public float strongDamage = 10f;
    private float damage;

    public float lightAttackCooldown = 1f;
    public float strongAttackCooldown = 2f;

    private float lastLightAttackTime = 0f;
    private float lastStrongAttackTime = 0f;

    private int attackNumber;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastLightAttackTime + lightAttackCooldown)
            LightAttack();

        else if (Input.GetMouseButtonDown(1) && Time.time >= lastStrongAttackTime + strongAttackCooldown)
            StrongAttack();
    }

    private void LightAttack()
    {
        attackNumber = 0;
        damage = lightDamage;
        lastLightAttackTime = Time.time;
        GameManager.instance.audioManager.Play("player_attack");
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackNumber", attackNumber);
    }

    private void StrongAttack()
    {
        attackNumber = 1;
        damage = strongDamage;
        lastStrongAttackTime = Time.time;
        GameManager.instance.audioManager.Play("player_attack");
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackNumber", attackNumber);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyController hitEnemy = other.gameObject.GetComponent<EnemyController>();
            hitEnemy.TakeDamage(damage);
        }
    }
}
