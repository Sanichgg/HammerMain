using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

[Serializable]
public class WaveSettings
{
    public SpawnerSidesEnum side;
    public EnemyTypeAndRarity EnemyTypeAndRarity;
    public float timeToSpawn;
    public int numberOfEnemies;
}
