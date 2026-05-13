using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour, IDamageable
{
    [Header("Movement Settings")]
    public float normalSpeed = 10f;
    public float rotateSpeed = 10f;

    [Header("Combat Settings")]
    public int maxHp = 5;
    public int currentHp;

    public Animator animator;
    private Rigidbody rigidbody;

    Dictionary<DragonStateType, IDragonState> dragonStates = new();

    IDragonState dragonState;

    public DragonStateType CurState => dragonState != null ? dragonState.state : DragonStateType.None;

    public Vector3 inputDirection { get; set; }

    #region Unit Methoed

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

    // Update is called once per frame
    void Update()
    {
        dragonState?.StateUpdate();
    }

    private void FixedUpdate()
    {
        dragonState?.StateFixedUpdate();
    }

    #endregion

    void InitFSM()
    {
        dragonStates = new()
        {
            { DragonStateType.Idle, new IdleDragonState(this)},
            { DragonStateType.Move, new MoveDragonState(this)},
            { DragonStateType.BasicAttack, new IdleDragonState(this)},
            { DragonStateType.Flame, new MoveDragonState(this)},
            { DragonStateType.Claw, new IdleDragonState(this)},
            { DragonStateType.Hit, new MoveDragonState(this)},
            { DragonStateType.Dead, new DeadDragonState(this)}
        };
    }

    public void SetState(DragonStateType newState)
    {
        if (CurState == newState) return;
        if (!dragonStates.ContainsKey(newState)) return;

        Debug.Log($"{newState}로 상태 변경");

        dragonState?.StateExit();
        dragonState = dragonStates[newState];
        dragonState.StateEnter();
    }

    public void ApplyDamage(int damage)
    {

    }
}
