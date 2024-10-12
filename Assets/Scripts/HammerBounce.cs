using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBounce : MonoBehaviour
{
    public int bounce;
    public bool isOnGround = false;
    private bool hasDealtDamage = false; //Для ситуаций когда нужно нанести урон ТОЛЬКО один раз

    HammerUse hammerUse;

    void Start()
    {
        hammerUse = GameObject.Find("HammerControll").GetComponent<HammerUse>();
        hammerUse.initialPos = transform.position;
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bounce++;

        if (bounce >= 5)
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy") && !hasDealtDamage)
        {
            CalculateDamage();
            hasDealtDamage = true; 
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = true;
            StartCoroutine(DestroyAfterDelay(3f));
        }

        if (collision.gameObject.CompareTag("Nasos") && !hasDealtDamage)
        {
            CalculateDamage();
            NasosMain nasos = collision.gameObject.GetComponent<NasosMain>();

            if (nasos != null)
            {
                nasos.ApplyDamage(hammerUse.finalDamage);
            }

            hasDealtDamage = true; 
        }

        if (collision.gameObject.CompareTag("Schetchik") && !hasDealtDamage)
        {
            CalculateDamage();
            SchetchikMain Schetchik = collision.gameObject.GetComponent<SchetchikMain>();

            if (Schetchik != null)
            {
                Schetchik.ApplyDamage(hammerUse.finalDamage);
            }

            hasDealtDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            isOnGround = false;
            StopCoroutine("DestroyAfterDelay");
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isOnGround)
        {
            Destroy(this.gameObject);
            isOnGround = false;
        }
    }

    private void CalculateDamage()
    {
        hammerUse.throwDistance = Vector3.Distance(hammerUse.initialPos, transform.position);
        hammerUse.addDamage = Mathf.Lerp(0, hammerUse.maxAdditionalDamage, hammerUse.throwDistance / hammerUse.maxDistance);
        hammerUse.damage = hammerUse.baseDamage + hammerUse.addDamage;
        hammerUse.finalDamage = Mathf.RoundToInt(hammerUse.damage);
    }
}
