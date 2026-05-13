using System.Collections;
using UnityEngine;

public class RightClickSkill : SkillBase
{
    public Projectile projectile;

    float preDelay = 0.7f;
    float postDelay = 0.5f;

    protected override void OnStart()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(preDelay);

        _owner.animator.SetAnim(3);
        yield return new WaitForSeconds(postDelay);
        Instantiate(projectile, transform.position, transform.rotation);

        Finish();
    }

    protected override void OnFinish()
    {

    }
}
