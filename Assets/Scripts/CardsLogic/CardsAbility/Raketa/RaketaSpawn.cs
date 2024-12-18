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

    public Transform spawnPoint; 
    public float screenXMin = -10f; 
    public float screenXMax = 10f;  
    public float raketaSpeed = 5f;

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
                SpawnRaketa();
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

    public void SpawnRaketa()
    {
        if (hasActivated) return;

        if (spawnPoint != null)
        {
            float randomX = Random.Range(screenXMin, screenXMax);
            Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z);
            GameObject spawnedSprite = Instantiate(raketaSprite, spawnPosition, Quaternion.identity);

            float rotationAngle;
            if (randomX < 0) 
            {
                rotationAngle = Random.Range(-170f, -120f);
            }
            else 
            {
                rotationAngle = Random.Range(-190f, -240f);
            }
            RaketaWork movementScript = spawnedSprite.AddComponent<RaketaWork>();
            movementScript.raketaSpawn = this;
            movementScript.Initialize(rotationAngle, raketaSpeed);

            hasActivated = true;
        }
    }

    public void ResetTimer()
    {
        secondsCounter = 0;
        timeSinceLastSecond = 0f;
        hasActivated = false;
    }
}
