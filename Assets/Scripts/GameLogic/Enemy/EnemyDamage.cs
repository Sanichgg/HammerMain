using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public HammerUse hammerUse;
    public MarshrutkaMove marshrutka;
    public svyatogorDamage svyatogor;
    public pianoLogicCard piano;
    public MineWork mine;
    public RaketaWork raketa;

    public EnemyBase enemyBase;
    private void Start()
    {
        hammerUse = GameObject.Find("HammerControll").GetComponent<HammerUse>();
        enemyBase = GetComponent<EnemyBase>();
    }

    private void FixedUpdate()
    {
        marshrutka = FindObjectOfType<MarshrutkaMove>();
        svyatogor = FindObjectOfType<svyatogorDamage>();
        piano = FindObjectOfType<pianoLogicCard>();
        mine = FindObjectOfType<MineWork>();
        raketa = FindObjectOfType<RaketaWork>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hammer"))
        {
            enemyBase.TakeDamage();
            enemyBase.damageText.gameObject.SetActive(true);
            enemyBase.damageText.text = hammerUse.finalDamage.ToString();
            StartCoroutine(HideText(1f));
        }


        if (collision.gameObject.CompareTag("Marshrutka"))
        {
            enemyBase.TakeDamageMarshurtka();
            enemyBase.damageText.gameObject.SetActive(true);
            enemyBase.damageText.text = marshrutka.finalMarshrutkaDamage.ToString();
            StartCoroutine(HideText(1f));
        }

        if (collision.gameObject.CompareTag("Svyatogor"))
        {
            enemyBase.TakeDamageSvyatogor();
            enemyBase.damageText.gameObject.SetActive(true);
            enemyBase.damageText.text = svyatogor.svyatogorDamageFinale.ToString();
            StartCoroutine(HideText(1f));
        }

        if (collision.gameObject.CompareTag("Piano"))
        {
            enemyBase.TakeDamagePiano();
            enemyBase.damageText.gameObject.SetActive(true);
            enemyBase.damageText.text = piano.finalPianoDamage.ToString();
            StartCoroutine(HideText(1f));
        }

        if (collision.gameObject.CompareTag("Mines"))
        {
            int damage = collision.gameObject.GetComponent<MineWork>().finalDamage;
            enemyBase.TakeDamageMines(damage);

            enemyBase.damageText.gameObject.SetActive(true);
            enemyBase.damageText.text = damage.ToString();

            StartCoroutine(HideText(1f));
        }

        if (collision.gameObject.CompareTag("Raketa"))
        {
            int damage = collision.gameObject.GetComponent<RaketaWork>().finalDamage;
            enemyBase.TakeDamageRaketa(damage);

            enemyBase.damageText.gameObject.SetActive(true);
            enemyBase.damageText.text = damage.ToString();

            StartCoroutine(HideText(1f));
        }
    }


    IEnumerator HideText(float time)
    {
        yield return new WaitForSeconds(time);
        enemyBase.damageText.gameObject.SetActive(false);
    }
}
