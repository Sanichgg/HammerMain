using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private svyatogorScript svyatogor; // Ссылка на скрипт Святогора
    private bool isSvyatogorActive = false; 

    void Start()
    {
        svyatogor = GetComponent<svyatogorScript>();
    }

    void Update()
    {
        if (isSvyatogorActive)
        {
            svyatogor.AttackEnemy(); 
            isSvyatogorActive = false; 
        }
    }

    public void ActivateSvyatogor()
    {
        isSvyatogorActive = true; 
    }
}

