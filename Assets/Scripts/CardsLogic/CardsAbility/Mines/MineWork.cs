using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MineWork : MonoBehaviour
{
    private GameObject targetLine;
    private bool targetLineSet = false;
    public GameObject boomPrefab;
    public GameObject explosionRadius;
    public float maxDamage = 25f;
    public float minDamage = 5f;
    public int finalDamage;
    private Rigidbody2D rb;
    public float hammerPushForce = 2f;

    private bool hasExploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (explosionRadius != null)
        {
            explosionRadius.SetActive(false);
        }
    }

    public void SetTargetLine(GameObject line)
    {
        targetLine = line;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == targetLine)
        {
            targetLineSet = true;

            rb.velocity = Vector2.zero;
            rb.isKinematic = true;

            Vector3 newPosition = transform.position;
            newPosition.y = targetLine.transform.position.y;
            transform.position = newPosition;
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hammer") && targetLineSet)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        if (collision.CompareTag("Enemy") && !hasExploded)
        {
            if (explosionRadius != null)
            {
                explosionRadius.SetActive(true);
            }

            ApplyExplosionEffects();
            Boom();
            hasExploded = true;
            StartCoroutine(DestroyMineAfterDelay(gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("hammer") && targetLineSet)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!targetLineSet || hasExploded) return;

        if (collision.CompareTag("Mines"))
        {
            Boom();
            StartCoroutine(DestroyMineAfterDelay(gameObject));
        }

        if (collision.CompareTag("hammer"))
        {
            float horizontalInput = collision.gameObject.transform.position.x - transform.position.x;
            rb.velocity = new Vector2(horizontalInput * hammerPushForce, rb.velocity.y);
        }
    }

    void Boom()
    {
        boomPrefab.SetActive(true);
    }

    private void ApplyExplosionEffects()
    {
        float radius = explosionRadius.GetComponent<Collider2D>().bounds.size.x / 2;
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objectsInRange)
        
            if (obj.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, obj.transform.position);
                finalDamage = Mathf.RoundToInt(CalculateDamage(distance, radius));
                Debug.Log("Дамаг: " + finalDamage);

                EnemyBase enemy = obj.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamageMines(finalDamage);
                }
                /*бум бум тута
             Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
             if (objRb != null)
             {
                 Vector2 explosionDirection = (obj.transform.position - transform.position).normalized;
                 objRb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);
             }
             */
            }
        
    }

    public float CalculateDamage(float distance, float radius)
    {
        if (distance <= radius)
        {
            float damage = maxDamage * (1 - (distance / radius));
            damage = Mathf.Clamp(damage, minDamage, maxDamage);
            return damage;
        }
        return 0;
    }

    private IEnumerator DestroyMineAfterDelay(GameObject mine)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(mine);
    }
}
