using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterBam : MonoBehaviour
{
    EnemyBase enemy;
    private void Update()
    {
        enemy = GetComponent<EnemyBase>();
        if(enemy.HP > 0)
        {
            Debug.Log("Log");
        }
    }
}
