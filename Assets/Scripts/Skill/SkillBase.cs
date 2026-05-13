using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected PlayerController _owner;

    public void Init(PlayerController owner)
    {
        _owner = owner;
    }

    public void Use()
    {
        _owner.SetState(PlayerStateType.Attack);
        OnStart();
    }

    public void Finish()
    {
        OnFinish();
        _owner.SetState(PlayerStateType.Idle);
    }

    protected abstract void OnStart();
    protected abstract void OnFinish();
}
