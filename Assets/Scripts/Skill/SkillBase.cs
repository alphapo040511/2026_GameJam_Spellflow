using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public string displayName;
    protected PlayerController _owner;
    protected Coroutine routine;

    // 🔥 핵심: 다음 스킬 테이블
    protected Dictionary<InputType, SkillBase> nextSkills = new();

    public void Init(PlayerController owner)
    {
        _owner = owner;
        OnInit();
    }

    protected virtual void OnInit() { }

    // 🔥 콤보 연결 등록
    public void AddNext(InputType input, SkillBase next)
    {
        nextSkills[input] = next;
    }

    // 🔥 다음 스킬 조회
    public SkillBase GetNext(InputType input)
    {
        if (nextSkills.TryGetValue(input, out SkillBase next))
            return next;

        return null;
    }
    private string GetInputName(InputType input)
    {
        return input switch
        {
            InputType.Light => "좌클",
            InputType.Heavy => "우클",
            InputType.LightHeavy => "좌클 + 우클",
            _ => "?"
        };
    }

    public string GetInputGuide()
    {
        if (nextSkills == null || nextSkills.Count == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder();

        foreach (var pair in nextSkills)
        {
            sb.Append(GetInputName(pair.Key));
            sb.Append(" → ");
            sb.Append(pair.Value.displayName);
            sb.Append('\n');
        }

        return sb.ToString();
    }

    // 🔥 실행
    public void Use()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Run());
    }

    protected abstract IEnumerator Run();

    public void Cancel()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        OnCancel();
    }

    protected virtual void OnCancel() { }
}