using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoController : MonoBehaviour
{
    public GameObject piano; // объект пиано
    private int secondsCounter = 0; // счётчик секунд
    private float timeSinceLastSecond = 0f; // отслеживает время для полных секунд
    private float baseChance = 1.0f; // начальный шанс
    private bool hasActivated = false; // активировалось ли пиано
    public PlayerController playerController; // ссылка на PlayerController
    public Transform[] linePoints;
    private GameObject activePiano;

    void Start()
    {
        secondsCounter = 0;
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        linePoints = new Transform[2];
        linePoints[0] = GameObject.Find("RightPoint_piano").transform;
        linePoints[1] = GameObject.Find("LeftPoint_piano").transform;
    }

    void Update()
    {
        UpdateTimer();
    }

    public void UpdateTimer()
    {
        if (!playerController.isPianoCardChosen || activePiano != null)
            return; //не обновляю таймер, если уже есть активное пиано

        timeSinceLastSecond += Time.deltaTime;

        //проверяем возможность активации способности
        if (timeSinceLastSecond >= 1f)
        {
            secondsCounter++;
            timeSinceLastSecond -= 1f; // уменьшаем на 1 секунду

            if (!hasActivated && ShouldActivate())
            {
                AttackEnemy();
            }
        }
    }

    public bool ShouldActivate()
    {
        //ограничиваем шанс активации
        float activationChance = Mathf.Clamp(baseChance + secondsCounter, 1f, 100f);
        float randomValue = Random.Range(0f, 100f);

        Debug.Log(activationChance);

        return randomValue <= activationChance;
    }

    public void AttackEnemy()
    {
        if (activePiano != null)
            return; // Если пиано уже активно, ничего не делаем

        int randomIndex = Random.Range(0, linePoints.Length);
        Transform spawnPoint = linePoints[randomIndex];
        activePiano = Instantiate(piano, spawnPoint.position, Quaternion.identity);
        hasActivated = true; // Устанавливаем флаг активации после спавна
        ResetActivationChance();
        playerController.pianoCardPoint++;
    }

    public void PianoDestroyed()
    {
        activePiano = null;
        ResetActivationChance();
    }

    private void ResetActivationChance()
    {
        secondsCounter = 0;
        hasActivated = false; //сброс флага активации
        baseChance = 1.0f; //сброс шанса к 1% после спавна пиано
    }
}
