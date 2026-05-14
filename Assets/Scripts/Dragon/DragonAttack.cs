using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    public Collider damageCollider;
    public float delay = 1.0f;
    public float duration = 0.1f;

    public int damage = 10;

    float startTime;
    private void OnEnable()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= startTime + delay + duration)
        {
            damageCollider.enabled = false;
        }
        else if (Time.time >= startTime + delay)
        {
            damageCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.ApplyDamage(damage);
        }
    }
}
