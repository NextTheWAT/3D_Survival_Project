using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : IState where T : MonoBehaviour
{
    protected T Component;

    public BaseState(T component)
    {
        Component = component;
    }

    public abstract void Start();
    public abstract void Update();
    public abstract void End();
}
