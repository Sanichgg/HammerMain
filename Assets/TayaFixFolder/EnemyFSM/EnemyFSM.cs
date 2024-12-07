using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyFSM : MonoBehaviour
{
    private EnemyFSMState StateCurrent { get; set; }

    private Dictionary<Type, EnemyFSMState> _states = new Dictionary<Type, EnemyFSMState>();

    public void AddState(EnemyFSMState state)
    {
        _states.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : EnemyFSMState
    {
        var type = typeof(T);

        if(StateCurrent.GetType() == type)
        {
            return;
        }

        if(_states.TryGetValue(type, out var newState))
        {
            StateCurrent?.Exit();
            StateCurrent = newState;
            StateCurrent.Enter();
        }
    }

    public void Update()
    {
        StateCurrent.Update();
    }
}
