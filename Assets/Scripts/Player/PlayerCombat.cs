using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;

    public SkillBase light1;
    public SkillBase light2;
    public SkillBase light3;
    public SkillBase heavy1;
    public SkillBase heavy2;

    InputBuffer inputBuffer;

    SkillBase currentSkill;

    float lastAttackTime;
    float keepDuration = 1f;

    Action SkillStop;

    public void StateEnter()
    {
        if(Time.time >= lastAttackTime + keepDuration)
        {
            currentSkill = null;
        }
    }

    public void StateExit()
    {
        lastAttackTime = Time.time;
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        inputBuffer = new InputBuffer();

        light1.Init(controller);
        light2.Init(controller);
        light3.Init(controller);
        heavy1.Init(controller);
        heavy2.Init(controller);

        SkillStop += light1.Cancel;
        SkillStop += light2.Cancel;
        SkillStop += light3.Cancel;
        SkillStop += heavy1.Cancel;
        SkillStop += heavy2.Cancel;

        SetupSkills();
    }

    void SetupSkills()
    {
        // L
        light1.AddNext(InputType.Light, light2);        // Light → Light (연타)
        light1.AddNext(InputType.Heavy, heavy1);        // Light → Heavy

        // L2
        light2.AddNext(InputType.Light, light3);
        light2.AddNext(InputType.Heavy, heavy1);

        // L3
        light3.AddNext(InputType.Light, light1);        // 반복

        // H1
        heavy1.AddNext(InputType.Light, light1);
        heavy1.AddNext(InputType.Heavy, heavy2);

        // H2
        heavy2.AddNext(InputType.Light, light1);

    }

    void Update()
    {
        if (controller.CurState != PlayerStateType.Idle &&
            controller.CurState != PlayerStateType.Move)
            return;

        inputBuffer.Update();

        bool l = Input.GetMouseButtonDown(0);
        bool r = Input.GetMouseButtonDown(1);

        if (l || r)
            inputBuffer.AddFromRawInput(l, r);

        Resolve();
    }

    void Resolve()
    {
        if (controller.CurState != PlayerStateType.Idle &&
            controller.CurState != PlayerStateType.Move)
            return;

        if (!inputBuffer.TryConsumeLatest(out InputType input))
            return;

        // 🔥 현재 스킬이 없으면 Light가 시작점
        if (currentSkill == null)
        {
            currentSkill = light1;
            currentSkill.Use();
            return;
        }

        // 🔥 다음 스킬 찾기
        SkillBase next = currentSkill.GetNext(input);

        if (next != null)
        {
            currentSkill = next;
            currentSkill.Use();
            return;
        }

        // ❗ fallback (콤보 깨짐)
        currentSkill = GetDefault(input);
        currentSkill.Use();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (currentSkill == null)
        {
            Debug.Log(light1.GetInputGuide());
            return;
        }

        string guide = currentSkill.GetInputGuide();
        Debug.Log(guide);
    }

    SkillBase GetDefault(InputType input)
    {
        return input switch
        {
            InputType.Light => light1,
            InputType.Heavy => heavy1,
            InputType.LightHeavy => light1,
            _ => light1
        };
    }

    public void CancelSkill()
    {
        SkillStop?.Invoke();
    }
}