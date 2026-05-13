using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    public void SetAnim(int index)
    {
        animator.SetInteger("State", index);
    }
}
