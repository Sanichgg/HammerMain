using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketaWork : MonoBehaviour
{
    private float speed;
    public GameObject explosionRadius;
    public float maxDamage = 40f;
    public float minDamage = 15f;
    public int finalDamage;
    private Rigidbody2D rb;
    public GameObject boomPrefab;
    private bool hasExploded = false;
    private bool isMoving = true;

    public RaketaSpawn raketaSpawn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        explosionRadius = transform.Find("Radius").gameObject;
        boomPrefab = transform.Find("boom").gameObject;
        if (explosionRadius != null)
        {
            explosionRadius.SetActive(false);
        }
        if (explosionRadius == null)
        {
            Debug.LogError("радиус не присвоен в инспекторе!");
        }
    }
    public void Initialize(float angle, float moveSpeed)
    {
        speed = moveSpeed;
        transform.rotation = Quaternion.Euler(0f, 0f, angle); 
    }


    void Update()
    {
        if (isMoving)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hammer"))
        {
            StopRaketa();
            Boom();
            StartCoroutine(DestroyRaketaAfterDelay(gameObject));
        }

        if (collision.CompareTag("Enemy") && !hasExploded)
        {
            if (explosionRadius != null)
            {
                explosionRadius.SetActive(true);
            }
            StopRaketa();
            ApplyExplosionEffects();
            Boom();
            hasExploded = true;
            StartCoroutine(DestroyRaketaAfterDelay(gameObject));
        }

        if (collision.CompareTag("Floor"))
        {
            StopRaketa();
            Boom();
            StartCoroutine(DestroyRaketaAfterDelay(gameObject));
        }
    }

    void Boom()
    {
        boomPrefab.SetActive(true);
    }

    private void ApplyExplosionEffects()
    {
        float radius = explosionRadius.GetComponent<Collider2D>().bounds.size.x;
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
                    enemy.TakeDamageRaketa(finalDamage);
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
        Debug.Log($"Расстояние: {distance}, Радиус: {radius}");
        if (distance <= radius)
        {
            float damage = maxDamage * (1 - (distance / radius));
            damage = Mathf.Clamp(damage, minDamage, maxDamage);
            return damage;
        }
        return 0;
    }

    private IEnumerator DestroyRaketaAfterDelay(GameObject raketa)
    {
        yield return new WaitForSeconds(0.5f);

        if (raketaSpawn != null)
        {
            raketaSpawn.ResetTimer();
        }
        Destroy(raketa);
    }

    void StopRaketa()
    {
        isMoving = false;  
        rb.velocity = Vector2.zero;  
    }

   

    
}
