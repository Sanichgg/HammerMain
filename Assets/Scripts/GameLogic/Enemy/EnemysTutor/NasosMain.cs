using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NasosMain : MonoBehaviour
{
    public Transform topPosition;
    public Transform bottomPosition;
    public float downSpeed = 2f;//скорость опускания
    public float riseSpeed = 1f;//скорость подъема
    public float requiredDamage = 15f;

    public int currentDamage = 0;
    public bool isFixedInPlace = false;
    private bool isGoingDown = false;
    private bool isGoingUp = false;
    private bool isForceDropping = false;
    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    public SpriteRenderer nasosSpriteRenderer; 
    public SpriteRenderer nasosBaseSpriteRenderer; 
    public Color normalColor = Color.red; 
    public Color filledColor = Color.green; 
    public Color baseNormalColor = Color.red;  
    public Color baseFilledColor = Color.green; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        targetPosition = initialPosition;
        nasosSpriteRenderer.color = normalColor;
        nasosBaseSpriteRenderer.color = baseNormalColor;
    }

    void Update()
    {
        if (!isFixedInPlace)
        {
            if (isForceDropping || isGoingDown)
            {
                transform.position = Vector3.Lerp(transform.position, bottomPosition.position, downSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, bottomPosition.position) < 0.01f)
                {
                    transform.position = bottomPosition.position;
                    isGoingDown = false;
                    isForceDropping = false;
                    if (currentDamage >= requiredDamage)
                    {
                        isFixedInPlace = true;
                        rb.bodyType = RigidbodyType2D.Static;
                        nasosSpriteRenderer.color = filledColor;
                        nasosBaseSpriteRenderer.color = baseFilledColor;
                    }
                    else
                    {
                        isGoingUp = true;
                    }
                }
            }
            else if (isGoingUp)
            {
                transform.position = Vector3.Lerp(transform.position, initialPosition, riseSpeed * Time.deltaTime);
                float riseProgress = Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(initialPosition, targetPosition);
                currentDamage = Mathf.CeilToInt(requiredDamage * (1 - riseProgress));

                if (currentDamage <= 0 || Vector3.Distance(transform.position, initialPosition) < 0.01f)
                {
                    isGoingUp = false;
                    currentDamage = 0;
                    nasosSpriteRenderer.color = normalColor;
                    nasosBaseSpriteRenderer.color = baseNormalColor;
                }
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        if (isFixedInPlace)
            return;

        currentDamage += damage;
        if (currentDamage >= requiredDamage)
        {
            currentDamage = (int)requiredDamage;
            isForceDropping = true;
            isGoingDown = false;
            isGoingUp = false;
            return;
        }

        float damageRatio = Mathf.Clamp((float)currentDamage / requiredDamage, 0f, 1f);
        float targetY = Mathf.Lerp(initialPosition.y, bottomPosition.position.y, damageRatio);
        targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        isGoingDown = true;
        isGoingUp = false;
    }
}
