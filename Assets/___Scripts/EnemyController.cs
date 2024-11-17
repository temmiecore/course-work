using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter), typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Header("Killbox reference")]
    public KillboxController killbox;

    [Header("Enemy stats")]
    public float health = 100f;
    public float damage = 1f;
    public float attackCooldown = 1f;
    public LootTable lootTable;

    private float attackTimeStamp = 0f;
    private bool isAttacking = false; // New flag to track if an attack is in progress

    private GameObject player;

    [Header("AI Radiuses")]
    [Tooltip("Radius for random movement when idle.")]
    public float movementRadius = 10f;
    [Tooltip("Radius to detect player.")]
    public float detectionRadius = 15f;
    [Tooltip("Distance to trigger an attack.")]
    public float attackDistance = 2f;

    [Header("Debug")]
    [SerializeField] private Vector3 idleTarget;
    [SerializeField] private bool isChasing = false;
    [SerializeField] private float distanceToPlayer;
    private bool isDead = false;

    private Seeker seeker;
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    private Animator animator;

    private Material material;
    private float dissolveAmount = 0;
    private float dissolveTime = 1f;

    void Start()
    {
        player = GameManager.instance.player;

        seeker = GetComponent<Seeker>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();

        material = FindMaterial();
        material.SetFloat("_Dissolve_Amount", 0);

        SetRandomIdleTarget();
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("No player object found in scene");
            return;
        }

        if (isDead)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (!isChasing && distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
            destinationSetter.target = player.transform;
        }
        else if (isChasing && distanceToPlayer > detectionRadius * 2)
        {
            isChasing = false;
            SetRandomIdleTarget();
        }

        /// Add wait time between setting another target
        if (!isChasing)
        {
            if (Vector3.Distance(transform.position, idleTarget) < 0.2f)
            {
                SetRandomIdleTarget();
            }
            else
            {
                destinationSetter.target = null;
                seeker.StartPath(transform.position, idleTarget);
            }
        }

        // Initiate an attack if in range and cooldown has passed, but not if already attacking
        if (isChasing && distanceToPlayer <= attackDistance && attackTimeStamp <= Time.time && !isAttacking)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(Attack());
            attackTimeStamp = Time.time + attackCooldown;
        }

        if (aiPath.velocity.magnitude > 0.1f)
            animator.SetBool("IsWalking", true);
        else
            animator.SetBool("IsWalking", false);
    }

    private Material FindMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material foundMaterial = null;

        if (renderer != null && renderer.material != null)
            foundMaterial = renderer.material;
        else
        {
            // If no material on the current object, check children
            bool materialFoundInChildren = false;
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer childRenderer in childRenderers)
            {
                if (childRenderer.material != null)
                {
                    foundMaterial = childRenderer.material;
                    Debug.Log("Material found on a child object: " + foundMaterial.name);
                    materialFoundInChildren = true;
                    break;
                }
            }

            if (!materialFoundInChildren)
                Debug.LogWarning("No material found on the object or its children.");
        }
        return foundMaterial;
    }

    private void SetRandomIdleTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movementRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;
        idleTarget = randomDirection;
        seeker.StartPath(transform.position, idleTarget);
    }
    
    IEnumerator Attack()
    {
        isAttacking = true;
        killbox.ResetDamageFlag(); 
        killbox.collider.enabled = true;
        yield return new WaitForSeconds(1f); // Attack animation duration
        killbox.collider.enabled = false;
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        GameManager.instance.audioManager.Play("enemy_hit");
        if (health <= 0)
        {
            GameManager.instance.audioManager.Play("enemy_die");
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        destinationSetter.target = null;
        animator.speed = 0f;

        float elapsedTime = 0;
        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / dissolveTime);
            material.SetFloat("_Dissolve_Amount", dissolveAmount);
            yield return null;
        }

        DropItems();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        if (lootTable == null)
            return;

        List<GameObject> droppedItems = lootTable.GetDrop();
        foreach (GameObject item in droppedItems)
        {
            // Make items "fly" out of the enemy
            Instantiate(item, transform.position, transform.rotation);
        }
    }

    // To draw radiuses
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
