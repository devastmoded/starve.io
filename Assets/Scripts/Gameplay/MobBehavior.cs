using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class MobBehavior : MonoBehaviour
{
    public int maxHealth = 100, damage = 50;
    public float MoveUpdateInteval = 1.5f, moveSpeed = 2f, runSpeedMultiplier = 1f, maxViewDistance = 5;
    public bool hostile;
    [Space]
    public Item[] drops;
    public int[] amounts;

    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D player, me;
    private Vector2 currentDir, targetDir;
    private float lastUpdateTime, despawnDist = 35, currentMoveSpeed;
    private int turnCount, turnSmooth = 35, currentHealth, overlaps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<CircleCollider2D>();
        me = GetComponent<CircleCollider2D>();
        lastUpdateTime = Time.time;
        currentMoveSpeed = moveSpeed;
        currentHealth = maxHealth;

        if (hostile) MobSpawner.hostileCount++;
        else MobSpawner.passiveCount++;
    }

    void FixedUpdate()
    {
        if ((player.transform.position - transform.position).magnitude >= despawnDist) Despawn();

        if (Time.time == lastUpdateTime + MoveUpdateInteval)
        {
            lastUpdateTime = Time.time;

            if (hostile && TouchingPlayer()) BarManager.ChangeValue(BarManager.Value.Health, -damage);
            if ((player.transform.position - transform.position).magnitude <= maxViewDistance && CanSeePlayer())
            {
                targetDir = (player.transform.position - transform.position).normalized;
                if (!hostile)
                {
                    currentMoveSpeed = runSpeedMultiplier * moveSpeed;
                    targetDir = -targetDir;
                }
            }
            else
            {
                targetDir = Random.insideUnitCircle.normalized;
                currentMoveSpeed = moveSpeed;
            }

            currentDir = -transform.up;
            turnCount = turnSmooth;

            if (targetDir != Vector2.zero) rb.velocity = currentMoveSpeed * targetDir;
        }

        if (turnCount > 0)
        {
            turnCount--;

            float angle = Vector2.Angle(currentDir, targetDir);
            if (Vector3.Cross(currentDir, targetDir).z < 0) angle *= -1;
            float turnAngle = 1.0f / turnSmooth * angle;
            transform.Rotate(Vector3.forward, turnAngle);
        }
    }

    private bool TouchingPlayer()
    {
        if ((transform.position - player.transform.position).magnitude - me.radius - player.radius <= 0) return true;
        else return false;
    }

    private bool CanSeePlayer()
    {
        int mask = LayerMask.GetMask("Player") + LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, mask);

        if (hit.collider != null && hit.collider.tag.Equals("Player")) return true;
        else return false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            for (int i = 0; i < drops.Length; i++) InvManager.UpdateSlot(drops[i], amounts[i]);
            Despawn();
        }
        else animator.Play("Damage");
    }

    public void Despawn()
    {
        if (hostile) MobSpawner.hostileCount--;
        else MobSpawner.passiveCount--;
        Destroy(gameObject);
    }
}