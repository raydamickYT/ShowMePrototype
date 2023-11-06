using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected GameManager Manager;
    protected FSM<GameManager> fSM;
    protected State(GameManager _manager,  FSM<GameManager> _fSM)
    {
        Manager = _manager;
        fSM = _fSM;
    }

    //protected FSM owner;
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

    private void OnDestroy() {
        fSM = null;
        Manager = null;
    }
}
