using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pianoLogicCard : MonoBehaviour
{
    public float speed = 5f;
    public float oscillationAmplitude = 0.5f;
    public float oscillationSpeed = 2f;

    public float pianoDamage;
    public HammerUse hammerUse;
    public PlayerController playerController;
    public int finalPianoDamage;

    public Sprite fallingPianoSprite;
    public Sprite brokenPianoSprite;

    private Vector3 direction;
    private bool isMoving = true;
    private bool hasLeftScreen = false;
    private Vector3 initialPosition;

    private PolygonCollider2D pianoCollider;
    private Renderer pianoRenderer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public PianoController pianoManager;

    void Start()
    {
        hammerUse = GameObject.Find("HammerControll").GetComponent<HammerUse>();
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        pianoManager = GameObject.Find("PlayerController").GetComponent<PianoController>();

        pianoDamage = hammerUse.baseDamage * 100 + (playerController.pianoCardPoint - 1 * 200);
        finalPianoDamage = Mathf.RoundToInt(pianoDamage);

        direction = transform.position.x < 0 ? Vector3.right : Vector3.left;

        initialPosition = transform.position;
        pianoCollider = GetComponent<PolygonCollider2D>();
        pianoRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            float newY = initialPosition.y + Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (hasLeftScreen)
            {
                if (!pianoRenderer.isVisible)
                {
                    isMoving = false;
                    Destroy(gameObject);
                }
            }
            else
            {
                if (pianoRenderer.isVisible)
                {
                    hasLeftScreen = true;
                    //Destroy(gameObject);
                }
            }
        }
    }

    public void StopPiano()
    {
        isMoving = false;
        rb.isKinematic = false;
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;

        spriteRenderer.sprite = fallingPianoSprite;

        StartCoroutine(DestroyPianoAfterDelay(2f));

        //GameObject ballom = GameObject.FindGameObjectWithTag("Balloom");
        //if (ballom != null)
        //{
        //    Destroy(ballom);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            spriteRenderer.sprite = brokenPianoSprite;
            rb.isKinematic = true;
            rb.gravityScale = 0;
        }
    }

    private IEnumerator DestroyPianoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        pianoManager.PianoDestroyed();
    }
}
