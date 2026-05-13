using UnityEngine;

public class IdleDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Idle;
    DragonController _owner;
    public IdleDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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

public class MoveDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Move;
    DragonController _owner;
    public MoveDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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
public class AttackDragonState : IDragonState
{

    public DragonStateType state => DragonStateType.BasicAttack;
    DragonController _owner;
    public AttackDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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
public class FlameDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Flame;
    DragonController _owner;
    public FlameDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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
public class ClawDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Claw;
    DragonController _owner;
    public ClawDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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

public class HitDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Hit;
    DragonController _owner;
    public HitDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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

public class DeadDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Dead;
    DragonController _owner;
    public DeadDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {

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