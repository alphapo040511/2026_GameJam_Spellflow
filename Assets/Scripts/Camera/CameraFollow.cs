using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("카메라 설정")]
    public Vector3 offset = new Vector3(0, 45, -30);
    public float followSpeed = 3f;

    [Header("대상 지정")]
    public PlayerController player;
    void Awake()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (player == null || player.CurState == PlayerStateType.Dead) return;

        Vector3 targetPos = player.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
