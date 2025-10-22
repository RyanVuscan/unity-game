using UnityEngine;

public class StateMachine
{
    public State Current { get; private set; }

    public void ChangeState(State next)
    {
        if (Current == next) return;
        Current?.Exit();
        Current = next;
        Current?.Enter();
    }

    public void Update()
    {
        Current?.Update();
    }
}
