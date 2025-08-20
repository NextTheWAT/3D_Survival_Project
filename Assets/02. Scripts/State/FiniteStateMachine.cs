using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    private List<IState> _states;
    private IState _currentState;

    public FiniteStateMachine(List<IState> states)
    {
        _states = states;
    }

    public void Run()
    {
        _currentState = _states[0];
        Update();
    }

    public void ChangeTo(int index)
    {
        _currentState.End();
        _currentState = _states[index];
        _currentState.Start();
    }

    public void Update()
    {
        _currentState.Update();
    }
}
