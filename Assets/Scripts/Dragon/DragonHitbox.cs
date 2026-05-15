using UnityEngine;

public class DragonHitbox : MonoBehaviour, IDamageable
{
    public DragonController controller;
    public void ApplyDamage(int damage)
    {
        controller.ApplyDamage(damage);
    }
}
