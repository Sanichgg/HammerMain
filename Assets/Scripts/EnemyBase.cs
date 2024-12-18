using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBase : MonoBehaviour
{
    [Header("Ragdoll")]
    //[SerializeField] GameObject ragdollBody;
    //private ragdoll2dfix ragdollScript;
    public EnemyDamage enemyDamage;

    private MeshRenderer meshRenderer;
    private SkeletonAnimation skeletonAnimation;

    [Header("HP system Settings")]
    private float hp = 30;
    public float maxHP;
    public TextMeshPro damageText;

    //public float damageEnemy;
    public HammerUse hammerUse;
    public float HP
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
    [SerializeField]  private int bigDamage = 13;

    private Rigidbody2D rb;

    private float xScale;
    private float currentXScale;

    [SerializeField] Transform handPosition;
    public bool isLeader = false;

    /*
    public MarshrutkaMove marshrutka;
    public svyatogorDamage svyatogor;
    public pianoLogicCard piano;
    */
    


    private void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();
        rb = GetComponent<Rigidbody2D>();
        //ragdollScript = ragdollBody.GetComponent<ragdoll2dfix>();
        meshRenderer = GetComponent<MeshRenderer>();
        speed = FindObjectOfType<EnemyManagerForTestBuild>().speed;

        canMove = true;
        tower = FindObjectOfType<Tower>().gameObject.transform;
        startPosition = transform.position.x;

        xScale = Mathf.Abs(transform.localScale.x);

        hammerUse = GameObject.FindObjectOfType<HammerUse>(); 

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
                damageText.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
                damageText.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (Mathf.Abs(transform.position.x) > 13f)
        {
            Destroy(gameObject);
        }
        /*
        marshrutka = FindObjectOfType<MarshrutkaMove>();
        svyatogor = FindObjectOfType<svyatogorDamage>();
        piano = FindObjectOfType<pianoLogicCard>();*/
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
        /*
        if (collision.gameObject.CompareTag("hammer"))
        {
            Destroy(collision.gameObject);
            TakeDamage();
            Debug.Log(hammerUse.damage);
        }
        */
        if (collision.gameObject.GetComponent<Brick>() && !carriesABrick)
        {
            carriesABrick = true;
            currentBrick = collision.gameObject.GetComponent<Brick>();
            TakeABrick();
        }
        //else if(collision.gameObject.GetComponent<Player>())
        //{
        //    carriesABrick = true;
        //}
        /*
        if (collision.gameObject.CompareTag("Marshrutka"))
        {
            //Destroy(collision.gameObject);
            StartCoroutine(Ragdoll());
            //Debug.Log(marshrutka.finalMarshrutkaDamage);
        }
        //else if(collision.gameObject.GetComponent<Player>())
        //{
        //    carriesABrick = true;
        //}

        if (collision.gameObject.CompareTag("Svyatogor"))
        {
            StartCoroutine(Ragdoll());
        }

        if (collision.gameObject.CompareTag("Piano"))
        {
            StartCoroutine(Ragdoll());
        }*/
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

        // if is final brick
        if (currentBrick != null && currentBrick.isFinalHammer)
        {
            //FindObjectOfType<EnemyManager>().Final(facingRight);
            FindObjectOfType<EnemyManagerForTestBuild>().Final(facingRight);
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private IEnumerator DeathTimer()
    {
        canMove = false;
        //GetComponent<SkeletonRagdoll2D>().Apply();

        if (carriesABrick)
        {
            carriesABrick = false;
        }

        yield return new WaitForSeconds(1);
        Death();
    }

    private IEnumerator Ragdoll()
    {
        
        if (carriesABrick)
        {
            carriesABrick = false;
        }

        //Debug.Log(skeletonAnimation != null);
        canMove = false;

        GetComponent<SkeletonAnimation>().AnimationName = "GetUp3";
        
        //GetComponent<SkeletonAnimation>().timeScale = 0; //////////////////////////////////////

        //skeletonAnimation.AnimationName = "GetUp";

        //ActivateRagdoll();
        
        //GetComponent<SkeletonAnimation>().AnimationName = "Idle";
        GetComponent<SkeletonRagdoll2D>().Apply();

        

        yield return new WaitForSeconds(2);
        Death();

        //if (!isDead)
        //{
        //    canMove = true;
        //    GetComponent<SkeletonRagdoll2D>().isActive = false;

        //    //GetComponent<SkeletonAnimation>().timeScale = 1; ////////////////////////////////////////

        //    GetComponent<SkeletonAnimation>().loop = true;
        //    GetComponent<SkeletonAnimation>().ClearState();
        //    GetComponent<SkeletonAnimation>().AnimationName = "Walk2";
        //}
    }

    //private void ActivateRagdoll()
    //{
    //    meshRenderer.enabled = false;
    //    ragdollBody.SetActive(true);

    //    ragdollScript.EnableRagdoll();
    //}
    //private void DeactivateRagdoll()
    //{
    //    meshRenderer.enabled = true;
    //    ragdollBody.SetActive(false);
    //    ragdollScript.DisableRagdoll();
    //}

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
        HP += -enemyDamage.hammerUse.finalDamage;
        if (/*hammerUse.damage > bigDamage*/ HP>=0)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            //StartCoroutine(Death());
        }
    }


    public void TakeDamageMarshurtka()
    {
        HP += -enemyDamage.marshrutka.finalMarshrutkaDamage;
        if (/*enemyDamage.marshrutka.marshrutkaDamage*/ HP >= 0)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            //StartCoroutine(Death());
        }
    }

    public void TakeDamageSvyatogor()
    {
        HP += -enemyDamage.svyatogor.svyatogorDamageFinale;
        if (/*enemyDamage.svyatogor.svyatogorDamageFinale*/ HP >= 0)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            //StartCoroutine(Death());
        }
    }

    public void TakeDamagePiano()
    {
        HP += -enemyDamage.piano.finalPianoDamage;
        if (/*enemyDamage.piano.finalPianoDamage*/HP >= 0)
        {
            StartCoroutine(Ragdoll());
        }

        if (HP <= 0)
        {
            isDead = true;
            //StartCoroutine(Death());
        }
    }

    public void TakeDamageMines(int damage)
    {
        HP -= damage;
        damageText.text = damage.ToString();
        if (damage > bigDamage)
        {
            StartCoroutine(Ragdoll());
        }
        if (HP <= 0)
        {
            isDead = true;
            //StartCoroutine(Death());
        }
    }

    public void TakeDamageRaketa(int damage)
    {
        HP -= damage;
        damageText.text = damage.ToString();
        if (damage > bigDamage)
        {
            StartCoroutine(Ragdoll());
        }
        if (HP <= 0)
        {
            isDead = true;
            //StartCoroutine(Death());
        }
    }
    public void TheVictoryMarch(bool leaderFacingRight)
    {
        Debug.Log("Victory at All Cost1");
        if (leaderFacingRight != facingRight)
        {
            Debug.Log("Victory at All Cost2");
            Flip();
        }
    }
}