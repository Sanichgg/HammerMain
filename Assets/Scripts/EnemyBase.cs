using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBase : MonoBehaviour
{
    [Header("HP system Settings")]
    private int hp = 30;
    public int maxHP;

    //public float damageEnemy;
    public HammerUse hammerUse;
    public MarshrutkaMove marshrutka;
    public svyatogorDamage svyatogor;
    public pianoLogicCard piano;
    public int HP
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, maxHP); }
    }

    [Header("Movement Settings")]
    [SerializeField] private Transform tower;

    public float speed;
    private float startPosition;
    bool facingRight = true;
    bool canMove;

    [Header("ActionStatus")]
    private bool carriesABrick = false;
    private Brick currentBrick = null;

    private bool isRagdoll;
    private bool isDead = false;
    private int bigDamage = 13;

    private Rigidbody2D rb;

    private float xScale;
    private float currentXScale;

    [SerializeField] Transform handPosition;

    //https://yandex.ru/video/preview/14501694929795046945 на ragdoll
    //https://ru.stackoverflow.com/questions/700172/%D0%9D%D0%B0%D0%B9%D1%82%D0%B8-%D0%B1%D0%BB%D0%B8%D0%B6%D0%BD%D0%B8%D0%B9-%D0%BE%D0%B1%D1%8A%D0%B5%D0%BA%D1%82-unity?ysclid=ltau7nueld728826623
    //ближайшая часть экрана

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = FindObjectOfType<EnemyManager>().speed;

        canMove = true;
        tower = FindObjectOfType<Tower>().gameObject.transform;
        startPosition = transform.position.x;

        xScale = Mathf.Abs(transform.localScale.x);

        hammerUse = GameObject.Find("HammerControll").GetComponent<HammerUse>();
        

        if (startPosition < tower.position.x)
        {
            facingRight = true;
            currentXScale = xScale;
        }
        else
        {
            facingRight = false;
            currentXScale = -xScale;
        } 
        transform.localScale = new Vector3(currentXScale, transform.localScale.y, transform.localScale.z);
    }

    void FixedUpdate()
    {
        if(canMove)  
        {
            if (facingRight)
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            }
        }

        if (Mathf.Abs(transform.position.x) > 13f)
        {
            Destroy(gameObject);
        }

        marshrutka = FindObjectOfType<MarshrutkaMove>();
        svyatogor = FindObjectOfType<svyatogorDamage>();
        piano = FindObjectOfType<pianoLogicCard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.GetComponent<Tower>())
        //{
        //    if (!carriesABrick)
        //    {
        //        //TakeABrick();
        //    }
        //}

        if (collision.gameObject.CompareTag("hammer"))
        {
            //Destroy(collision.gameObject);
            TakeDamage();
            //Debug.Log(hammerUse.finalDamage);
        }

        if (collision.gameObject.GetComponent<Brick>() && !carriesABrick)
        {
            carriesABrick = true;
            currentBrick = collision.gameObject.GetComponent<Brick>();
            TakeABrick();
        }

        if (collision.gameObject.CompareTag("Marshrutka"))
        {
            //Destroy(collision.gameObject);
            TakeDamageMarshurtka();
            //Debug.Log(marshrutka.finalMarshrutkaDamage);
        }
        //else if(collision.gameObject.GetComponent<Player>())
        //{
        //    carriesABrick = true;
        //}

        if (collision.gameObject.CompareTag("Svyatogor"))
        {
            TakeDamageSvyatogor();
        }

        if (collision.gameObject.CompareTag("Piano"))
        {
            TakeDamagePiano();  
        }
    }

    private void TakeABrick()
    {
        currentBrick.InHands(handPosition);
        StartCoroutine(TakeABrickTimer());
    }

    private void LostABrick()
    {
        currentBrick.Stolen();
        currentBrick = null;
        carriesABrick = false;
    }

    private IEnumerator TakeABrickTimer()
    {
        GetComponent<SkeletonAnimation>().AnimationName = "Steal Tower";
        GetComponent<SkeletonAnimation>().loop = false;
        canMove = false;

        yield return new WaitForSeconds(1);
        
        carriesABrick = true;
        Flip();
        canMove = true;

        if (carriesABrick)
        {
            GetComponent<SkeletonAnimation>().loop = true;
            GetComponent<SkeletonAnimation>().AnimationName = "WalkTower";
        }
        else
        {
            GetComponent<SkeletonAnimation>().loop = true;
            GetComponent<SkeletonAnimation>().AnimationName = "Walk2";
        }
    }

    private IEnumerator Death()
    {
        canMove = false;

        GetComponent<SkeletonRagdoll2D>().Apply();

        if (carriesABrick)
        {
            carriesABrick = false;
        }

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator Ragdoll()
    {
        if (carriesABrick)
        {
            carriesABrick = false;
        }
        GetComponent<SkeletonAnimation>().AnimationName = "Idle";
        //GetComponent<SkeletonRagdoll2D>().Apply();

        canMove = false;

        yield return new WaitForSeconds(2);

        if (!isDead)
        {
            canMove = true;
            GetComponent<SkeletonRagdoll2D>().isActive = false;

            GetComponent<SkeletonAnimation>().loop = true;
            GetComponent<SkeletonAnimation>().ClearState();
            GetComponent<SkeletonAnimation>().AnimationName = "Walk2";
        }
    }

    private void Flip()
    {
        if (facingRight)
        {
            facingRight = false;
            currentXScale = xScale;
        }
        else
        {
            facingRight = true;
            currentXScale = -xScale;
        }
        float newXscale = -transform.localScale.x;
        transform.localScale = new Vector3(newXscale, transform.localScale.y, transform.localScale.z);
    }

    public void TakeDamage()
    {
        HP += -hammerUse.finalDamage;
        if (hammerUse.finalDamage > bigDamage)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }

    public void TakeDamageMarshurtka()
    {
        HP += -marshrutka.finalMarshrutkaDamage;
        if (marshrutka.marshrutkaDamage > bigDamage)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }

    public void TakeDamageSvyatogor()
    {
        HP += -svyatogor.svyatogorDamageFinale;
        if (svyatogor.svyatogorDamageFinale > bigDamage)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }

    public void TakeDamagePiano()
    {
        HP += -piano.finalPianoDamage;
        if (piano.finalPianoDamage > bigDamage)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }
}