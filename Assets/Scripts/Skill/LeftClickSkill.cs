using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class LeftClickSkill : SkillBase
{
    public Projectile projectile;

    public float preDelay = 0.1f;
    public float postDelay = 0.8f;

    protected override IEnumerator Run()
    {
        _owner.SetState(PlayerStateType.Attack);

        yield return new WaitForSeconds(preDelay);

        _owner.animator.SetAnim(3);

        yield return new WaitForSeconds(postDelay);

        // 🔥 실제 공격 타이밍
        Instantiate(projectile, transform.position, transform.rotation);

        _owner.SetState(PlayerStateType.Idle);

        routine = null;
    }

    protected override void OnCancel()
    {

    }
}