using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Player Settings")]
    public float normalSpeed = 10f;
    public float rotateSpeed = 10f;

    [Header("Combat Settings")]
    public int maxHp = 5;
    public int currentHp;
    public Image hpBar;

    [Header("Camera Settings")]
    public Transform cameraTransform;

    [Header("Ground Settings")]
    [SerializeField] float groundCheckDistance = 1.2f;
    [SerializeField] float groundSnapSpeed = 10f;
    [SerializeField] LayerMask groundMask;
    public PlayerAnim animator { get; set; }
    private Rigidbody rigidbody;

    public PlayerCombat PlayerCombat;

    public bool invincibility = false;

    Dictionary<PlayerStateType, IPlayerState> playerStates = new();

    IPlayerState playerState;

    public PlayerStateType CurState => playerState != null ? playerState.state : PlayerStateType.None;

    public Vector3 inputDirection { get; set; }

    #region Unit Methoed

    void Awake()
    {
        animator = GetComponent<PlayerAnim>();
        rigidbody = GetComponent<Rigidbody>();
        InitFSM();
    }

    void Start()
    {
        currentHp = maxHp;
        SetState(PlayerStateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        playerState?.StateUpdate();
        HandleInput();
    }

    private void FixedUpdate()
    {
        playerState?.StateFixedUpdate();
        GroundSnap();
    }

    #endregion

    void InitFSM()
    {
        playerStates = new()
        {
            { PlayerStateType.Idle, new IdleState(this)},
            { PlayerStateType.Move, new MoveState(this)},
            { PlayerStateType.Roll, new RollState(this)},
            { PlayerStateType.Attack, new AttackState(this)},
            { PlayerStateType.Stun, new StunState(this)},
            { PlayerStateType.Dead, new DeadState(this)}
        };
    }

    public void SetState(PlayerStateType newState)
    {
        if (CurState == newState) return;
        if (!playerStates.ContainsKey(newState)) return;

        //Debug.Log($"{newState}로 상태 변경");

        playerState?.StateExit();
        playerState = playerStates[newState];
        playerState.StateEnter();
    }


    public void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // y축 제거
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * y + right * x;

        if (dir.sqrMagnitude >= 0.01f)
        {
            dir.Normalize();
            inputDirection = dir;
        }
        else
        {
            inputDirection = Vector3.zero;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (inputDirection.sqrMagnitude >= 0.01f)
                transform.forward = inputDirection;
            SetState(PlayerStateType.Roll);
        }
    }
    public void SetRigidLock()
    {
        if (rigidbody == null) return;

        rigidbody.linearVelocity = Vector3.zero;
    }

    public void Movement()
    {
        if (rigidbody != null)
        {
            Vector3 vel = inputDirection * normalSpeed;
            float yVel = rigidbody.linearVelocity.y;
            vel.y = yVel;
            rigidbody.linearVelocity = vel;
        }

        Rotate();
    }

    void GroundSnap()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.2f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundMask))
        {
            Vector3 pos = rigidbody.position;

            // 현재 y를 바닥 높이에 맞춰 보정
            float targetY = hit.point.y;

            pos.y = Mathf.Lerp(pos.y, targetY, Time.fixedDeltaTime * groundSnapSpeed);

            rigidbody.position = pos;
        }
    }

    public void MovementToDir(Vector3 dir, float speed)
    {
        if (rigidbody != null)
        {
            Vector3 vel = dir * speed;
            float yVel = rigidbody.linearVelocity.y;
            rigidbody.linearVelocity = vel;
        }
    }

    public void Rotate()
    {
        Quaternion targetRot = Quaternion.LookRotation(inputDirection, Vector3.up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            10f * Time.deltaTime
        );
    }

    public void LookForward()
    {
        Vector3 forward = cameraTransform.forward;

        // y축 제거
        forward.y = 0;

        forward.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(forward, Vector3.up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            10f * Time.deltaTime
        );
    }

    public void RotateToMouse()
    {
        Plane plane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPos = ray.GetPoint(distance);

            Vector3 dir = targetPos - transform.position;
            dir.y = 0;

            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    10f * Time.deltaTime
                );
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        if (invincibility) return;

        currentHp -= damage;
        hpBar.fillAmount = currentHp / (float)maxHp;
        if (currentHp <= 0) SetState(PlayerStateType.Dead);
    }
}
