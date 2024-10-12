using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchetchikMain : MonoBehaviour
{
    public GameObject strelka; // Стрелка
    public SpriteRenderer greenSchethik;// Зеленый спрайт счетчика
    public SpriteRenderer redSchethik; // Красный спрайт счетчика
    public SpriteRenderer greenBolt;// Зеленый болт
    public SpriteRenderer redBolt;// Красный болт

    public float rotateSpeed = 50f;// Стандартная скорость вращения стрелки
    public float slowDownSpeed = 10f;// Скорость вращения при замедлении (когда тип стрелка идет к значению который равен урону)
    public float recoverySpeed = 20f;// Скорость восстановления скорости
    
    public float requiredDamage = 15f;// Урон для полной фиксации счетчика
    public float currentDamage = 0f; // Текущий урон

    private float currentRotationSpeed;// Текущая скорость вращения
    private bool isDamaged = false;
    private bool isFixedInPlace = false;
    private float maxRotation = -360f;// Максимальный угол вращения

    void Start()
    {
        currentRotationSpeed = rotateSpeed;
        SetZRotation(0);

        greenSchethik.enabled = true;
        redSchethik.enabled = false;
        greenBolt.enabled = true;
        redBolt.enabled = false;
    }

    void Update()
    {
        float currentRotation = GetZRotation();
        if (isFixedInPlace)
        {
            greenSchethik.enabled = true;
            redSchethik.enabled = false;
            greenBolt.enabled = true;
            redBolt.enabled = false;
        }
        else
        {
            if (currentRotation <= -120f || currentRotation >= maxRotation + 360f)
            {
                greenSchethik.enabled = false;
                redSchethik.enabled = true;
                greenBolt.enabled = false;
                redBolt.enabled = true;
            }
            else
            {
                greenSchethik.enabled = true;
                redSchethik.enabled = false;
                greenBolt.enabled = true;
                redBolt.enabled = false;
            }
        }

        if (!isFixedInPlace)
        {
            if (isDamaged)
            {
                float newRotation = Mathf.MoveTowards(currentRotation, GetTargetRotation(), slowDownSpeed * Time.deltaTime);
                SetZRotation(newRotation);

                currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, rotateSpeed, recoverySpeed * Time.deltaTime);
                currentDamage = Mathf.MoveTowards(currentDamage, 0f, (requiredDamage / 10) * Time.deltaTime);

                if (currentDamage <= 0)
                {
                    isDamaged = false;
                }
            }
            else
            {
                currentRotation -= currentRotationSpeed * Time.deltaTime;

                if (currentRotation <= maxRotation)
                {
                    currentRotation += 360f;
                }

                SetZRotation(currentRotation);
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        if (isFixedInPlace) return;

        currentDamage += damage;
        isDamaged = true;
        currentRotationSpeed = slowDownSpeed;
        if (currentDamage >= requiredDamage)
        {
            currentDamage = requiredDamage;
            SetZRotation(0);
            isFixedInPlace = true;

            greenSchethik.enabled = true;
            redSchethik.enabled = false;
            greenBolt.enabled = true;
            redBolt.enabled = false;
        }
    }


    private float GetZRotation()
    {
        float zRotation = strelka.transform.localEulerAngles.z;
        if (zRotation > 180)
            zRotation -= 360;
        return zRotation;
    }

    private void SetZRotation(float angle)
    {
        strelka.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private float GetTargetRotation()
    {
        float damageRatio = currentDamage / requiredDamage;
        return Mathf.Lerp(0f, maxRotation, damageRatio);
    }
}
