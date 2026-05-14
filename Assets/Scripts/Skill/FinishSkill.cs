using System.Collections;
using UnityEngine;

public class FinishSkill : SkillBase
{
    public TopDown projectile;

    public float preDelay = 0.7f;
    public float postDelay = 0.8f;

    protected override IEnumerator Run()
    {
        _owner.SetState(PlayerStateType.Attack);

        yield return new WaitForSeconds(preDelay);

        _owner.animator.SetAnim(3);

        yield return new WaitForSeconds(postDelay);

        Instantiate(projectile, transform.position + Vector3.forward * 5f, transform.rotation);

        _owner.SetState(PlayerStateType.Idle);

        routine = null;
    }

    protected override void OnCancel()
    {

    }
}
