using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;

    [Header("스킬 정보")]
    public SkillBase leftSkill;
    public SkillBase rightSkill;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        if (controller == null) return;

        if (leftSkill != null) leftSkill.Init(controller);
        if (rightSkill != null) rightSkill.Init(controller);
    }

    void Update()
    {
        if (controller.CurState != PlayerStateType.Idle && controller.CurState != PlayerStateType.Move) return;

        if(Input.GetMouseButtonDown(0))
        {
            if (leftSkill != null)
                leftSkill.Use();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (rightSkill != null)
                rightSkill.Use();
        }
    }
}
