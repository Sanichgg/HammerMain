using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerUse : MonoBehaviour
{
    public GameObject objectPrefab;
    private GameObject currentObject;
    public bool useHammer = true;
    [Space]
    [Header("Throw Force")]
    private Vector3 mouseOffset;
    private bool isDragging = false;
    public float dropForce;

    public GameObject reloadHammer;//отдельный ГО на молоток-перезарядку
    private GameObject reloadHammerInstance;
    private bool isReloading = false;
    //public Color startColor = Color.blue;
    //public Color endColor = Color.green;
    //private SpriteRenderer circleSpriteRenderer;

    Vector3 throwVector;

    //For Roation When Throwing
    [Header("For Roation When Throwing")]
    Rigidbody2D currRb;
    public float strength = 100;
    public float rotX;
    public float rotY;
    bool applyForce;

    public bool dropHammer = false;

    private Vector2 currentPosPercent;
    private Vector2 prevMousePostPercent;

    //Magazine
    [Header("Magazine")]
    public int hammerCount = 20;
    public float reloadTime = 2.0f;
    public float reloadTimer = 0.0f;
    public int allHammers;

    public UIHammer uIHammer;

    //Dymanic Flight
    [Header("Damage")]
    public float baseDamage = 3f; //Базовый урон молота
    public float maxAdditionalDamage = 20f; //Максимум к прибавке урона 
    public float maxDistance = 50f; //Максимальная дистанция, которую может пролететь молот
    [Space]
    [Header("DON'T TOUCH! IT'S FOR REFERENCE!")]
    public float addDamage;
    public float throwDistance;
    public float damage;

    public Vector3 initialPos;
    public int finalDamage;

    void Start()
    {
        //StartCoroutine(GetPrev());

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hammerCount > 0 && reloadTimer <= 0 && useHammer ==  true)
        {
            //Instantiate hammer on mouse, get its rigidbody, controll only the current hammer
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentObject = Instantiate(objectPrefab, mousePosition, Quaternion.identity);
            currRb = currentObject.GetComponent<Rigidbody2D>();
            mouseOffset = currentObject.transform.position - mousePosition;
            isDragging = true;

            //Always check where the hammer will go if mouse up
            //CalculateThrowVector();

            initialPos = transform.position;

            hammerCount--;
            allHammers++;
        }

        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadHammerInstance != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;  
                reloadHammerInstance.transform.position = mousePosition;
                float reloadProgress = (reloadTime - reloadTimer) / reloadTime;
                //circleSpriteRenderer.color = Color.Lerp(startColor, endColor, reloadProgress);
            }
        }

        if (hammerCount <= 0 && reloadTimer <= 0)
        {
            reloadTimer = reloadTime;
            hammerCount = 20;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;  
            reloadHammerInstance = Instantiate(reloadHammer, mousePosition, Quaternion.identity);
            //circleSpriteRenderer = reloadHammerInstance.GetComponentInChildren<SpriteRenderer>();
            isReloading = true;
        }

        if (reloadTimer <= 0 && isReloading)
        {
            if (reloadHammerInstance != null)
            {
                Destroy(reloadHammerInstance);
            }

            isReloading = false;
        }


        if (isDragging && currentObject != null)
        {
            //Follow mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentObject.transform.position = mousePosition + mouseOffset;

            //Calculate the strength of the mouse for hammer rotation mid air
            applyForce = true;
            rotX = -Input.GetAxis("Mouse X") * strength;
            rotY = Input.GetAxis("Mouse Y") * strength;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(currentObject.transform.position, 0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Enemy")
                {
                    throwDistance = Vector3.Distance(initialPos, transform.position);
                    addDamage = Mathf.Lerp(0, maxAdditionalDamage, throwDistance / maxDistance);
                    damage = baseDamage + addDamage;
                    finalDamage = Mathf.RoundToInt(damage);

                    //enemyPosition = collider.transform.position;
                    //hammerDirection = (Vector2)transform.position - enemyPosition;
                    //throwDistance = hammerDirection.magnitude;
                    //damage = throwDistance * damageCoefficient;

                    Debug.Log(damage);
                    dropHammer = true;
                    
                    //Debug.Log(damage);
                    break;
                }
               
            }

            if (dropHammer)
            {
                isDragging = false;    
            }

            CalculateThrowVector();


        }
        else applyForce = false;

        if (Input.GetMouseButtonUp(0) && currentObject != null)
        {
            //Yaaay kill the capitalists
            isDragging = false;
            //Debug.Log(throwVector);
            currentObject.GetComponent<Rigidbody2D>().AddForce(throwVector);
            currentObject.GetComponent<Rigidbody2D>().AddTorque(-throwVector.x * 0.1f);
            dropHammer = false;
            currentObject = null;

           
            //Debug.Log(throwStartTime);
        }
    }

    private void FixedUpdate()
    {
        if (applyForce)
        {
           // currRb.AddTorque(-throwVector.x * 0.1f); //Applying force to rotate hammer mid air
        }
    }

    void CalculateThrowVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        currentPosPercent = new Vector2(mousePos.x/Screen.width, mousePos.y/Screen.height);

        throwVector = (currentPosPercent - prevMousePostPercent) * dropForce;
        prevMousePostPercent = currentPosPercent;
        //Debug.Log(throwVector);


        // Vector2 distance = mousePos - this.transform.position;
        // throwVector = -distance * 100;
    }

    IEnumerator GetPrev()
    {
        while (true)
        {
            prevMousePostPercent = currentPosPercent;
            yield return new WaitForSeconds(2f);
        }
    }


    

}
