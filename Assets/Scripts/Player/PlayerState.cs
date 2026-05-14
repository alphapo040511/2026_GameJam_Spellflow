
using System.Collections;
using UnityEngine;

public class IdleState : IPlayerState
{
    public PlayerStateType state => PlayerStateType.Idle;

    PlayerController _owner;

    public IdleState(PlayerController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.SetRigidLock();
        _owner.animator.SetAnim(0);
    }

    public void StateUpdate()
    {
        if (_owner.inputDirection.sqrMagnitude >= 0.01f)
        {
            _owner.SetState(PlayerStateType.Move);
        }
        //_owner.RotateToMouse();
    }

    public void StateFixedUpdate()
    {

    }

    public void StateExit()
    {

    }
}

public class MoveState : IPlayerState
{
    public PlayerStateType state => PlayerStateType.Move;

    PlayerController _owner;
    public MoveState(PlayerController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.animator.SetAnim(1);
    }

    public void StateUpdate()
    {
        if (_owner.inputDirection.sqrMagnitude < 0.01f)
        {
            _owner.SetState(PlayerStateType.Idle);
        }

        //_owner.RotateToMouse();
    }

    public void StateFixedUpdate()
    {
        _owner.Movement();
    }

    public void StateExit()
    {

    }
}

public class RollState : IPlayerState
{
    public PlayerStateType state => PlayerStateType.Roll;

    PlayerController _owner;


    float time = 0;
    Vector3 dir;
    float rollDuration = 0.8f;
    float invincibilityTime = 0.3f;
    float rollSpeed = 16f;

    public RollState(PlayerController owner)
    {
        _owner = owner;
    }
    public void StateEnter()
    {
        _owner.animator.SetAnim(7);
        _owner.invincibility = true;
        dir = _owner.transform.forward.normalized;
        time = 0;
    }

    public void StateUpdate()
    {
        
    }

    public void StateFixedUpdate()
    {
        _owner.MovementToDir(dir, rollSpeed);
        time += Time.fixedDeltaTime;

        if(time >= invincibilityTime)
        {
            _owner.invincibility = false;
        }

        if(time >= rollDuration)
        {
            _owner.SetState(PlayerStateType.Idle);
        }
    }

    public void StateExit()
    {
        _owner.invincibility = false;
    }
}

public class AttackState : IPlayerState
{
    public PlayerStateType state => PlayerStateType.Attack;

    PlayerController _owner;

    public AttackState(PlayerController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.SetRigidLock();
        _owner.animator.SetAnim(2);
        _owner.PlayerCombat.StateEnter();
    }

    public void StateUpdate()
    {
        //_owner.RotateToMouse();
        _owner.LookForward();
    }

    public void StateFixedUpdate()
    {

    }

    public void StateExit()
    {
        _owner.PlayerCombat.CancelSkill();
        _owner.PlayerCombat.StateExit();
    }
}

public class StunState : IPlayerState
{
    public PlayerStateType state => PlayerStateType.Stun;

    PlayerController _owner;
    public StunState(PlayerController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.SetRigidLock();
        //_owner.animator.SetAnim(2);
    }

    public void StateUpdate()
    {

    }

    public void StateFixedUpdate()
    {

    }

    public void StateExit()
    {

    }
}

public class DeadState : IPlayerState
{
    public PlayerStateType state => PlayerStateType.Dead;

    PlayerController _owner;
    public DeadState(PlayerController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.SetRigidLock();
        //_owner.animator.SetAnim(2);
    }

    public void StateUpdate()
    {
        
    }

    public void StateFixedUpdate()
    {

    }

    public void StateExit()
    {

    }
}
