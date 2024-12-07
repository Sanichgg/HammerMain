using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFSMState 
{
    protected readonly EnemyFSM Fsm;

    public EnemyFSMState (EnemyFSM fsm)
    {
        Fsm = fsm;
    }

    public virtual void Enter() { }
    
    public virtual void Exit() { }

    public virtual void Update() { }
}
