using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour, IDamageable
{
    [Header("Target")]
    public Transform player;

    [Header("Movement Settings")]
    public float normalSpeed = 10f;
    public float rotateSpeed = 10f;

    [Header("Combat Settings")]
    public int maxHp = 5;
    public int currentHp;

    public GameObject attack;
    public GameObject claw;
    public GameObject flame;

    public Animator animator;
    private Rigidbody rigidbody;
    public Rigidbody RB => rigidbody;
    public Transform Target => player;

    Dictionary<DragonStateType, IDragonState> dragonStates = new();
    IDragonState dragonState;

    public DragonStateType CurState => dragonState != null ? dragonState.state : DragonStateType.None;

    public Vector3 inputDirection { get; set; }

    private float actionInteraval = 4f;
    private float lastActionTime;

    bool playerFind = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        InitFSM();
    }

    void Start()
    {
        currentHp = maxHp;
        SetState(DragonStateType.Idle);
    }

    void Update()
    {
        dragonState?.StateUpdate();
        AIUpdate(); // ⭐ AI 판단 추가
    }

    void FixedUpdate()
    {
        dragonState?.StateFixedUpdate();
    }

    public bool isActionLocked = false;

    void AIUpdate()
    {
        if (player == null || CurState == DragonStateType.Dead) return;

        // 🔥 행동 중이면 AI 완전 정지
        if (isActionLocked) return;

        if (Time.time < lastActionTime + actionInteraval) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Idle 범위 밖
        if (dist > 50f && !playerFind)
        {
            return;
        }
        else if (!playerFind)
        {
            playerFind = true;
            animator.SetTrigger("Scream");
            lastActionTime = Time.time;
            return;
        }

        actionInteraval = 5;

        // 방향 바라보기 (항상)
        //Vector3 dir = (player.position - transform.position);
        //dir.y = 0;

        //if (dir != Vector3.zero)
        //{
        //    Quaternion rot = Quaternion.LookRotation(dir);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
        //}

        // =========================
        // 1) 근접 구간 (0~5)
        // =========================
        if (dist <= 23f)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
                SetState(DragonStateType.BasicAttack);
            else
                SetState(DragonStateType.Claw);
            lastActionTime = Time.time;
            return;
        }

        // =========================
        // 2) 중거리 구간 (5~15)
        // =========================
        if (dist <= 35f)
        {
            float rand = Random.value;

            // 🔥 30% 확률 브레스
            if (rand < 0.3f)
            {
                SetState(DragonStateType.Flame);
            }
            else
            {
                SetState(DragonStateType.Move); // 계속 추적하면서 접근
            }
            lastActionTime = Time.time;
            return;
        }

        // =========================
        // 3) 추적 구간 (15~50)
        // =========================
        if (dist <= 50f)
        {
            SetState(DragonStateType.Move);
        }
    }

    void InitFSM()
    {
        dragonStates = new()
        {
            { DragonStateType.Idle, new IdleDragonState(this)},
            { DragonStateType.Move, new MoveDragonState(this)},

            { DragonStateType.BasicAttack, new AttackDragonState(this)},
            { DragonStateType.Claw, new ClawDragonState(this)},
            { DragonStateType.Flame, new FlameDragonState(this)},

            { DragonStateType.Hit, new HitDragonState(this)},
            { DragonStateType.Dead, new DeadDragonState(this)}
        };
    }

    public void SetState(DragonStateType newState)
    {
        if (CurState == newState) return;
        if (!dragonStates.ContainsKey(newState)) return;

        dragonState?.StateExit();
        dragonState = dragonStates[newState];
        dragonState.StateEnter();
    }

    int damageSum = 0;

    public void ApplyDamage(int damage)
    {
        if (CurState == DragonStateType.Dead) return;

        currentHp -= damage;
        damageSum += damage;

        if (currentHp <= 0)
            SetState(DragonStateType.Dead);
        else if (damageSum >= 50 && !isActionLocked)
        {
            damageSum = 0;
            SetState(DragonStateType.Hit);
        }
    }
}