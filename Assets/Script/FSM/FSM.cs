using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    private State<T> currentState;
    private Dictionary<System.Type, State<T>> allStates = new Dictionary<System.Type, State<T>>();

    public void Initialize()
    {
        // heb deze gelaten voor het geval ik ooit nog iets wil initializen hier
    }

    public void AddState(State<T> _state)
    {
        allStates.Add(_state.GetType(), _state);
    }

    public void Update()
    {
        //de update functie binnen de states is een override functie die werkt als hij wordt aangeroepen.
        currentState?.OnUpdate();
    }

    public void SwitchState(System.Type _type)
    {
        currentState?.OnExit();
        currentState = allStates[_type];
        currentState?.OnEnter();
    }
    
    

}
