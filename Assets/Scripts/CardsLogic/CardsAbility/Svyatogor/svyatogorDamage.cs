using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class svyatogorDamage : MonoBehaviour
{
    public PlayerController playerController;
    public HammerUse hammerUse;
    public GameObject damageColl;
    public GameObject finalPosDamageColl;
    public float svyatogorDamages;
    public int svyatogorDamageFinale;
    public float fallSpeed = 25f;

    void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        hammerUse = GameObject.Find("HammerControll").GetComponent<HammerUse>();
        svyatogorDamages = hammerUse.baseDamage * 12000f;
        svyatogorDamageFinale = Mathf.RoundToInt(svyatogorDamages);
    }

    void Update()
    {
        if (damageColl != null && finalPosDamageColl != null)
        {
            if (damageColl.transform.position.y > finalPosDamageColl.transform.position.y)
            {
                damageColl.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            }
        }
    }
}
