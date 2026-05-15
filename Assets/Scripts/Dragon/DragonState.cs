using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IdleDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Idle;

    DragonController _owner;
    Rigidbody rb => _owner.RB;

    public IdleDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        // Animator: Idle
        _owner.animator.SetBool("Move", false);


        rb.linearVelocity = Vector3.zero;
    }

    public void StateUpdate() { }

    public void StateFixedUpdate()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public void StateExit() { }
}

public class MoveDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Move;

    DragonController _owner;
    Rigidbody rb => _owner.RB;
    Transform target => _owner.Target;
    float stopDistance = 18.0f;
    public MoveDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        // Animator: Run / Roar transition
        _owner.animator.SetBool("Move", true);
    }

    public void StateUpdate()
    {
        if (target == null) return;

        Vector3 dir = (target.position - _owner.transform.position);
        dir.y = 0;

        if (dir == Vector3.zero) return;

        // 회전
        Quaternion rot = Quaternion.LookRotation(dir);
        _owner.transform.rotation = Quaternion.Slerp(
            _owner.transform.rotation,
            rot,
            Time.deltaTime * _owner.rotateSpeed
        );
    }

    public void StateFixedUpdate()
    {
        if (target == null) return;

        Vector3 dir = (target.position - _owner.transform.position);
        dir.y = 0;

        float dist = dir.magnitude;

        // 🔥 너무 가까우면 멈춤
        if (dist <= stopDistance)
        {
            _owner.animator.SetBool("Move", false);
            rb.linearVelocity = Vector3.zero;
            return;
        }
        else
            _owner.animator.SetBool("Move", true);

        dir.Normalize();

        // 이동
        Vector3 move = dir * _owner.normalSpeed;
        rb.linearVelocity = move;
    }

    public void StateExit()
    {
        _owner.animator.SetBool("Move", false);
    }
}
public class AttackDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.BasicAttack;

    DragonController _owner;
    Rigidbody rb => _owner.RB;

    float timer;

    public AttackDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        // Animator: Bite
        _owner.animator.SetTrigger("Attack1");
        _owner.isActionLocked = true;
        rb.linearVelocity = Vector3.zero;
        _owner.attack.SetActive(true);
        timer = 2f; // 공격 지속 시간
    }

    public void StateUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            _owner.SetState(DragonStateType.Move);
        }
    }

    public void StateFixedUpdate() { }

    public void StateExit()
    {
        _owner.isActionLocked = false;
        _owner.attack.SetActive(false);
    }
}
public class FlameDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Flame;

    DragonController _owner;
    Rigidbody rb => _owner.RB;

    float timer;

    public FlameDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        // Animator: Breath
        _owner.animator.SetTrigger("Flame");
        _owner.isActionLocked = true;
        rb.linearVelocity = Vector3.zero;
        timer = 0;
    }

    public void StateUpdate()
    {
        timer += Time.deltaTime;

        _owner.flame.SetActive(timer >= 0.6f);

        if (timer >= 4)
        {
            _owner.SetState(DragonStateType.Move);
        }
    }

    public void StateFixedUpdate() { }

    public void StateExit() {
        _owner.isActionLocked = false;
        _owner.flame.SetActive(false);
    }
}
public class ClawDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Claw;

    DragonController _owner;
    Rigidbody rb => _owner.RB;

    float timer;

    public ClawDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.animator.SetTrigger("Claw");
        rb.linearVelocity = Vector3.zero;
        _owner.claw.SetActive(true);
        _owner.isActionLocked = true;
        timer = 3;
    }

    public void StateUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            _owner.SetState(DragonStateType.Move);
        }
    }

    public void StateFixedUpdate() { }

    public void StateExit()
    {
        _owner.isActionLocked = false;
        _owner.claw.SetActive(false);
    }
}

public class DashDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Dash;

    DragonController _owner;
    Rigidbody rb => _owner.RB;
    Transform target => _owner.Target;
    float stopDistance = 25.0f;

    float timer;

    public DashDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.animator.SetBool("Dash", true);
        rb.linearVelocity = Vector3.zero;
        _owner.claw.SetActive(true);
        _owner.isActionLocked = true;
        timer = 3;
    }

    public void StateUpdate()
    {

    }

    public void StateFixedUpdate() 
    {

        if (target == null) return;

        Vector3 dir = (target.position - _owner.transform.position);
        dir.y = 0;

        float dist = dir.magnitude;

        // 가까우면 멈춤
        if (dist <= stopDistance)
        {
            _owner.animator.SetBool("Dash", false);
            rb.linearVelocity = Vector3.zero;

            _owner.SetState(DragonStateType.Idle);

            return;
        }

        dir.Normalize();

        // 이동
        Vector3 move = dir * _owner.dashSpeed;
        rb.linearVelocity = move;
    }

    public void StateExit()
    {
        _owner.isActionLocked = false;
        _owner.claw.SetActive(false);
    }
}

public class HitDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Hit;

    DragonController _owner;
    Rigidbody rb => _owner.RB;

    float timer;

    public HitDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.animator.SetTrigger("Hit");
        rb.linearVelocity = Vector3.zero;
        _owner.isActionLocked = true;
        timer = 0.8f;
    }

    public void StateUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            _owner.SetState(DragonStateType.Move);
        }
    }

    public void StateFixedUpdate() { }

    public void StateExit() { _owner.isActionLocked = false; }
}

public class DeadDragonState : IDragonState
{
    public DragonStateType state => DragonStateType.Dead;

    DragonController _owner;
    Rigidbody rb => _owner.RB;

    public DeadDragonState(DragonController owner)
    {
        _owner = owner;
    }

    public void StateEnter()
    {
        _owner.animator.SetBool("Dead", true);
        rb.linearVelocity = Vector3.zero;
        _owner.isActionLocked = true;
    }

    public void StateUpdate() { }

    public void StateFixedUpdate() { }

    public void StateExit() { _owner.isActionLocked = false; }
}