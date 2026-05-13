using UnityEngine;

public enum DragonStateType
{
    None,
    Idle,
    Move,
    BasicAttack,
    Claw,
    Flame,
    Hit,
    Dead
}

public interface IDragonState
{
    public DragonStateType state { get; }
    public void StateEnter();
    public void StateUpdate();
    public void StateFixedUpdate();
    public void StateExit();
}
