using UnityEngine;

public abstract class State
{
    protected GameObject owner;
    protected StateMachine fsm;

    protected State(GameObject owner, StateMachine fsm)
    {
        this.owner = owner;
        this.fsm = fsm;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
