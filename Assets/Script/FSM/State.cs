﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    //protected FSM owner;
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}
