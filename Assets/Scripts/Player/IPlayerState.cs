using UnityEngine;

public interface IPlayerState
{
    public PlayerStateType state { get; }
    public void StateEnter();
    public void StateUpdate();
    public void StateFixedUpdate();
    public void StateExit();
}
