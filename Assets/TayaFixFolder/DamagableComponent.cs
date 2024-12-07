using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableComponent : MonoBehaviour
{ 
    [SerializeField] int hp = 100;

    int currentHp;
    bool isDead;

    private void Start()
    {
        currentHp = hp;
    }

    public bool IsDead => isDead;

    public int MaxHp => hp;
    public int Hp
    {
        get => currentHp;
        set
        {
            if (isDead)
                return;

            int delta = value - currentHp;
            currentHp = Mathf.Max(value, 0);

            if (currentHp <= 0)
                Death();
        }
    }

    public void AddHP(int delta)
    {
        hp += delta;
    }

    public void RemoveHP(int delta)
    {
        hp -= delta;
    }


    private void Update()
    {
        if (Hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        
    }

    private void OnEnable()
    {
        Hp = hp;
    }

    private void OnDisable()
    {
        
    }
}
