using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketaSpawn : MonoBehaviour
{
    public GameObject raketaSprite;
    private int secondsCounter = 0;
    private float timeSinceLastSecond = 0f;
    private float baseChance = 1.0f;
    private bool hasActivated = false;
    public PlayerController playerController;

    public Transform[] spawnPoints;
    public float maxRotationAngle = 30f; 

    // Start is called before the first frame update
    void Start()
    {
        secondsCounter = 0;
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    public void UpdateTimer()
    {
        if (!playerController.isRaketaCardChosen || hasActivated)
            return;

        timeSinceLastSecond += Time.deltaTime;

        if (timeSinceLastSecond >= 1f)
        {
            secondsCounter++;
            timeSinceLastSecond -= 1f; // уменьшаем на 1 секунду
            if (ShouldActivate())
            {
                AttackEnemy();
            }
        }
    }

    private bool ShouldActivate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return false; // если врагов нет, не активируем способность

        float activationChance = Mathf.Min(baseChance + secondsCounter, 100f);
        float randomValue = Random.Range(0f, 100f);
        Debug.Log(activationChance);
        return randomValue <= activationChance;
    }

    public void AttackEnemy()
    {
        if (hasActivated) return;

        if (spawnPoints.Length > 0)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 spawnPosition = randomSpawnPoint.position;

            GameObject spawnedSprite = Instantiate(raketaSprite, spawnPosition, Quaternion.Euler(0f, 0f, -180f)); 
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(spawnPosition, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            if (closestEnemy != null)
            {
                Vector3 direction = closestEnemy.transform.position - spawnPosition;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float deltaAngle = Mathf.Clamp(angle - (-180f), -maxRotationAngle, maxRotationAngle);
                float finalAngle = -180f + deltaAngle; // -180 градусов

                spawnedSprite.transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
            }

            StartCoroutine(DestroySpriteAfterDelay(spawnedSprite, 2f));
            hasActivated = true;
        }
    }

    private IEnumerator DestroySpriteAfterDelay(GameObject sprite, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(sprite);
        ResetTimer(); // сбрасываю таймер после уничтожени€ спрайта
    }

    private void ResetTimer()
    {
        secondsCounter = 0;
        timeSinceLastSecond = 0f;
        hasActivated = false; // сбрасываю флаг активации только после уничтожени€ спрайта
    }

    private bool IsVisible(Transform enemyTransform)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemyTransform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }
}
